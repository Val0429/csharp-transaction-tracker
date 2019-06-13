using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace TimeTrack
{
    public class GetPartArgument
    {
        public GetPartArgument()
        {

        }

        public GetPartArgument(ushort part)
        {
            Part = part;
        }

        public UInt16 Part { get; set; }
        public UInt64 Start { get; set; }
        public UInt64 End { get; set; }
    }

    public class Tracker : Label, ITracker
    {
        public IServer Server { get; set; }
        public TrackerContainer TrackerContainer { get; set; }

        public event EventHandler<EventArgs<Int32>> OnDragStart;
        public event EventHandler OnGetPartStart;
        public event EventHandler OnGetPartCompleted;
        public event EventHandler OnGetDownloadPartCompleted;

        protected ICamera _camera;
        public virtual ICamera Camera
        {
            get
            {
                return _camera;
            }
            set
            {
                if (_camera == value) return;

                if (_camera != value)
                {
                    Parts.Clear();
                    PlaybackParts.Clear();

                }

                _camera = value;

                if (_camera is IDeviceLayout)
                {
                    foreach (ICamera camera in ((IDeviceLayout)_camera).Items)
                    {
                        if (camera == null) continue;
                        _camera.Bookmarks = camera.Bookmarks;
                        break;
                    }
                }
                else if (_camera is ISubLayout)
                {
                    var id = SubLayoutUtility.CheckSubLayoutRelativeCamera((ISubLayout)_camera);
                    var camera = _camera.Server.Device.FindDeviceById(Convert.ToUInt16(id)) as ICamera;
                    if (camera != null)
                        _camera.Bookmarks = camera.Bookmarks;
                }

                if (_camera != null)
                {
                    ShowRecord();
                    ShowDownloadPart();
                }
                else
                {
                    _loadingEvent = false;
                    _getPartToBackQueue.Clear();
                    _getPartToFrontQueue.Clear();

                    if (_getPartBackgroundWorker.IsBusy)
                    {
                        _getPartBackgroundWorker.CancelAsync();
                    }

                    if (_getPartAddFrontBackgroundWorker.IsBusy)
                    {
                        _getPartAddFrontBackgroundWorker.CancelAsync();
                    }

                    if (_getPartAddBackBackgroundWorker.IsBusy)
                    {
                        _getPartAddBackBackgroundWorker.CancelAsync();
                    }

                    if (_getDownloadPartBackgroundWorker.IsBusy)
                    {
                        _getDownloadPartBackgroundWorker.CancelAsync();
                    }

                    if (OnGetPartCompleted != null)
                        OnGetPartCompleted(this, null);
                }
            }
        }

        public Boolean IsBusy
        {
            get
            {
                return (_camera != null && (_getPartAddFrontBackgroundWorker.IsBusy || _getPartAddBackBackgroundWorker.IsBusy ||
                        _getPartBackgroundWorker.IsBusy || _getPartToBackQueue.Count > 0 || _getPartToFrontQueue.Count > 0 || _loadingEvent));
            }
        }

        private Boolean _using = true;
        public Boolean Using
        {
            get { return _using; }
            set
            {
                _using = value;
                Visible = Using;
            }
        }

        public UInt16 Position
        {
            set
            {
                Location = new Point(0, TrackerContainer.TopPosition + value * TrackerContainer.TrackerHeight);
            }
        }

        private BackgroundWorker _getPartBackgroundWorker;
        private readonly BackgroundWorker _getPartAddFrontBackgroundWorker;
        private readonly BackgroundWorker _getPartAddBackBackgroundWorker;

        private BackgroundWorker _getDownloadPartBackgroundWorker;

        private readonly List<GetPartArgument> _getPartToBackQueue = new List<GetPartArgument>();
        private readonly List<GetPartArgument> _getPartToFrontQueue = new List<GetPartArgument>();
        protected Queue<Record> Parts = new Queue<Record>();
        public  Queue<Record> PlaybackParts = new Queue<Record>();
        private TimeUnit _previousTimeUnit;
        private UInt64 _previousStartTime;
        private UInt64 _previousEndTime;


        // Constructor
        public Tracker()
        {
            Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left);
            DoubleBuffered = true;
            //Dock = DockStyle.Top;
            BackColor = Color.Transparent;

            MouseDown += TrackerMouseDown;
            MouseUp += TrackerMouseUp;

            Paint += TrackerPaint;

            _getPartBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _getPartBackgroundWorker.DoWork -= GetPart;
            _getPartBackgroundWorker.RunWorkerCompleted -= GetPartCompleted;
            _getPartBackgroundWorker.DoWork += GetPart;
            _getPartBackgroundWorker.RunWorkerCompleted += GetPartCompleted;

            _getPartAddFrontBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _getPartAddFrontBackgroundWorker.DoWork -= GetPartAddFront;
            _getPartAddFrontBackgroundWorker.RunWorkerCompleted -= GetPartAddFrontCompleted;
            _getPartAddFrontBackgroundWorker.DoWork += GetPartAddFront;
            _getPartAddFrontBackgroundWorker.RunWorkerCompleted += GetPartAddFrontCompleted;

            _getPartAddBackBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _getPartAddBackBackgroundWorker.DoWork -= GetPartAddBack;
            _getPartAddBackBackgroundWorker.RunWorkerCompleted -= GetPartAddBackCompleted;
            _getPartAddBackBackgroundWorker.DoWork += GetPartAddBack;
            _getPartAddBackBackgroundWorker.RunWorkerCompleted += GetPartAddBackCompleted;

            _getDownloadPartBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _getDownloadPartBackgroundWorker.DoWork -= GetDownloadPart;
            _getDownloadPartBackgroundWorker.RunWorkerCompleted -= GetDownloadPartCompleted;
            _getDownloadPartBackgroundWorker.DoWork += GetDownloadPart;
            _getDownloadPartBackgroundWorker.RunWorkerCompleted += GetDownloadPartCompleted;
        }

        protected UInt64 CalculatorStart()
        {
            if (_camera == null) return 0;

            UInt64 startTime = TrackerContainer.StartTime;

            if (Server.Server.TimeZone != _camera.Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(startTime);
                time += (Server.Server.TimeZone * 1000);
                time -= (_camera.Server.Server.TimeZone * 1000);
                startTime = Convert.ToUInt64(time);
            }

            return startTime;
        }

        private Boolean _isCallShowRecord;
        public virtual void ShowRecord()
        {
            if (_camera == null) return;

            //when minize, don't waste time to draw parts data.
            if (TrackerContainer.IsMinimize) return;

            //Console.WriteLine(@"Get Parts : " + _camera);

            //Async get part
            UInt16 part = TrackerContainer.Parts;
            if (part == 0) return;

            UInt64 startTime = CalculatorStart();

            UInt64 endTime = startTime + TrackerContainer.PartsToTime(part);

            UInt16 mode = 0;
            if (Parts.Count != 0 && _previousTimeUnit == TrackerContainer.UnitTime)
            {
                //same duration
                if (_previousStartTime == startTime && endTime == _previousEndTime)
                {
                    mode = 0;//original value is 1, but it won't eraser the forefront record when times go by 
                }
                else if (_previousStartTime < startTime && _previousEndTime > startTime && endTime > _previousEndTime)
                {
                    mode = 1;
                    //Console.WriteLine("Add to back");
                    //Console.WriteLine("Before :" + (endTime - TrackerContainer.StartTime) / 1000 + " After : " + (endTime - _previousEndTime) / 1000);
                }
                else if (_previousStartTime < endTime && _previousEndTime > endTime && startTime < _previousStartTime)
                {
                    mode = 2;
                    //Console.WriteLine("Add to front");
                    //Console.WriteLine("Before :" + (endTime - TrackerContainer.StartTime) / 1000 + " After : " + (_previousStartTime - TrackerContainer.StartTime) / 1000);
                }
            }

            if (mode == 0)
            {
                _getPartToBackQueue.Clear();
                _getPartToFrontQueue.Clear();

                if (_getPartBackgroundWorker.IsBusy)
                {
                    _getPartBackgroundWorker.CancelAsync();
                    _getPartBackgroundWorker.Dispose();
                    _getPartBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
                    _getPartBackgroundWorker.DoWork += GetPart;
                    _getPartBackgroundWorker.RunWorkerCompleted += GetPartCompleted;
                }

                if (_getPartAddFrontBackgroundWorker.IsBusy)
                    _getPartAddFrontBackgroundWorker.CancelAsync();

                if (_getPartAddBackBackgroundWorker.IsBusy)
                    _getPartAddBackBackgroundWorker.CancelAsync();

                if (!_getPartBackgroundWorker.IsBusy && part != 0)
                {
                    _getPartBackgroundWorker.RunWorkerAsync(new GetPartArgument(part)
                    {
                        Start = startTime,
                        End = endTime,
                    });
                }
            }
            else if (mode == 1)//add to back  [old] < - [new]
            {
                Record lastRecord = Parts.LastOrDefault();
                if (lastRecord == null) return;
                _previousEndTime = Math.Min(_previousEndTime, DateTimes.ToUtc(lastRecord.EndDateTime, _camera.Server.Server.TimeZone));

                _previousEndTime = Math.Max(_previousEndTime, startTime);

                part = TrackerContainer.DurationToParts(_previousEndTime, endTime);

                if (part == 0) return;

                var shortEnd = _previousEndTime + TrackerContainer.PartsToTime(part);

                //already in cache
                if (Parts.Count == 0 || DateTimes.ToUtc(lastRecord.EndDateTime, _camera.Server.Server.TimeZone) < shortEnd)
                {
                    if (!_getPartAddBackBackgroundWorker.IsBusy)
                        _getPartAddBackBackgroundWorker.RunWorkerAsync(new GetPartArgument(part)
                        {
                            Start = _previousEndTime,
                            End = shortEnd,
                        });
                    else
                    {
                        _getPartToBackQueue.Add(new GetPartArgument(part)
                        {
                            Start = _previousEndTime,
                            End = shortEnd,
                        });
                    }
                }
            }
            else if (mode == 2)//add to front [new] -> [old]
            {
                //Console.WriteLine("Before Part :" + parts + " " + _camera.Name);
                part = TrackerContainer.DurationToParts(startTime, _previousStartTime);
                //Console.WriteLine("Part :" + parts);

                var shortStart = _previousStartTime - TrackerContainer.PartsToTime(part);
                //var count = Parts.Where(x => x.Type != EventType.VideoRecord).Count();
                //if (count == 0 || DateTimes.ToUtc(Parts.Peek().StartDateTime, _camera.Server.Server.TimeZone) > shortStart - (TrackerContainer.PartsToTime(part)* 1))//Parts.Count == 0 ||  
                //{
                    Console.WriteLine("In");
                    if (!_getPartAddFrontBackgroundWorker.IsBusy)
                    {
                        _getPartAddFrontBackgroundWorker.RunWorkerAsync(new GetPartArgument(part)
                            {
                                Start = shortStart,
                                End = _previousStartTime,
                            });
                    }
                    else
                    {
                        //Console.WriteLine("Queue Start " + _camera.Name + " " + shortStart);
                        _getPartToFrontQueue.Add(new GetPartArgument(part)
                        {
                            Start = shortStart,
                            End = _previousStartTime,
                        });
                    }
                //}
            }

            _previousTimeUnit = TrackerContainer.UnitTime;
            _previousStartTime = startTime;
            _previousEndTime = endTime;

            if (OnGetPartStart != null)
                OnGetPartStart(this, null);
        }

        public void ShowDownloadPart()
        {
            if (!_getDownloadPartBackgroundWorker.IsBusy)
                UpdateDownloadPart();
        }

        private void UpdateDownloadPart()
        {
            //when minize, don't waste time to draw parts data.
            if (TrackerContainer.IsMinimize) return;

            //no need update, timetrack is deactivate.
            if (!TrackerContainer.RefreshTracker) return;

            //if (!(Server is ICMS)) return;

            if (_camera == null) return;

            //Async get part
            UInt16 part = TrackerContainer.Parts;
            if (part == 0) return;

            UInt64 startTime = CalculatorStart();

            UInt64 endTime = startTime + TrackerContainer.PartsToTime(part);

            if (_getDownloadPartBackgroundWorker.IsBusy)
            {
                _getDownloadPartBackgroundWorker.CancelAsync();
                _getDownloadPartBackgroundWorker.Dispose();
                _getDownloadPartBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
                _getDownloadPartBackgroundWorker.DoWork -= GetDownloadPart;
                _getDownloadPartBackgroundWorker.RunWorkerCompleted -= GetDownloadPartCompleted;
                _getDownloadPartBackgroundWorker.DoWork += GetDownloadPart;
                _getDownloadPartBackgroundWorker.RunWorkerCompleted += GetDownloadPartCompleted;
            }

            _getDownloadPartBackgroundWorker.RunWorkerAsync(new GetPartArgument(part)
            {
                Start = startTime,
                End = endTime,
            });
        }

        public Record GetFocusRecord()
        {
            if (_camera == null || Parts.Count == 0) return null;

            return Parts.FirstOrDefault(record => record.StartDateTime <= TrackerContainer.DateTime && record.EndDateTime >= TrackerContainer.DateTime);
        }

        public Record GetRightNowRecord()
        {
            if (_camera == null || Parts.Count == 0) return null;

            return Parts.FirstOrDefault(record => record.StartDateTime <= TrackerContainer.DateTime && record.EndDateTime >= TrackerContainer.DateTime.AddSeconds(1));
        }

        public Record GetRecordFromDateTime()
        {
            if (_camera == null || Parts.Count == 0) return null;

            return Parts.FirstOrDefault(record => record.EndDateTime > TrackerContainer.DateTime);
        }

        public virtual void GetDownloadPart(Object sender, DoWorkEventArgs e)
        {
            var argument = e.Argument as GetPartArgument;
            if (argument == null || _camera == null)
            {
                return;
            }

            PlaybackParts = _camera.GetPlaybackDownloadPart(argument.Part, argument.Start, argument.End);
        }

        public virtual void GetPart(Object sender, DoWorkEventArgs e)
        {
            var argument = e.Argument as GetPartArgument;
            if (argument == null || _camera == null)
            {
                return;
            }

            Parts = _camera.GetParts(argument.Part, argument.Start, argument.End);

            var types = new List<EventType>(TrackerContainer.SearchEventType);
            foreach (var eventType in types)
            {
                if (!TrackerContainer.SearchEventType.Contains(eventType)) continue;

                var parts = _camera.GetEventParts(argument.Part, argument.Start, argument.End, eventType);

                if (!TrackerContainer.SearchEventType.Contains(eventType))
                {
                    var keepRecord = new List<Record>(Parts);
                    Parts.Clear();
                    foreach (var record in keepRecord)
                    {
                        if (record.Type == eventType) continue;

                        Parts.Enqueue(record);
                    }
                    continue;
                }

                foreach (var eventRecord in parts)
                    Parts.Enqueue(eventRecord);
            }
        }

        public virtual void GetPartAddFront(Object sender, DoWorkEventArgs e)
        {
            var argument = e.Argument as GetPartArgument;
            if (argument == null || _camera == null) return;

            var frontRecord = _camera.GetParts(argument.Part, argument.Start, argument.End);

            var types = new List<EventType>(TrackerContainer.SearchEventType);
            foreach (var eventType in types)
            {
                var parts = _camera.GetEventParts(argument.Part, argument.Start, argument.End, eventType);

                foreach (var eventRecord in parts)
                    frontRecord.Enqueue(eventRecord);
            }

            while (frontRecord.Count > 0)
            {
                if (Parts.Count == 0)
                {
                    Parts = frontRecord;
                    break;
                }

                Record newRecord = frontRecord.Dequeue();

                if (Parts.Any(record => record.EndDateTime == newRecord.EndDateTime &&
                    record.StartDateTime == newRecord.StartDateTime &&
                    record.Type == newRecord.Type))
                    continue;

                if (!Parts.Any(record => record.EndDateTime >= newRecord.StartDateTime &&
                    record.StartDateTime <= newRecord.EndDateTime &&
                    record.Type == newRecord.Type))
                {
                    Parts.Enqueue(newRecord);
                    continue;
                }

                foreach (var record in Parts)
                {
                    if (record.StartDateTime <= newRecord.EndDateTime &&
                        record.StartDateTime > newRecord.StartDateTime &&
                        record.Type == newRecord.Type)
                    {
                        record.StartDateTime = newRecord.StartDateTime;
                        break;
                    }
                }
            }

            var temp = new List<Record>(Parts.ToArray());
            temp.Sort((x, y) => x.StartDateTime.CompareTo(y.StartDateTime));
            Parts.Clear();
            foreach (var record in temp)
                Parts.Enqueue(record);
        }

        public virtual void GetPartAddBack(Object sender, DoWorkEventArgs e)
        {
            var argument = e.Argument as GetPartArgument;
            if (argument == null || _camera == null) return;

            var backRecord = _camera.GetParts(argument.Part, argument.Start, argument.End);
           
            var types = new List<EventType>(TrackerContainer.SearchEventType);
            foreach (var eventType in types)
            {
                var parts = _camera.GetEventParts(argument.Part, argument.Start, argument.End, eventType);

                foreach (var eventRecord in parts)
                    backRecord.Enqueue(eventRecord);
            }

            while (backRecord.Count > 0)
            {
                if (Parts.Count == 0)
                {
                    Parts = backRecord;
                    break;
                }

                Record newRecord = backRecord.Dequeue();

                if (Parts.Any(record => record.EndDateTime == newRecord.EndDateTime &&
                    record.StartDateTime == newRecord.StartDateTime &&
                    record.Type == newRecord.Type))
                    continue;

                if (!Parts.Any(record => record.EndDateTime >= newRecord.StartDateTime &&
                    record.StartDateTime <= newRecord.EndDateTime &&
                    record.Type == newRecord.Type))
                {
                    Parts.Enqueue(newRecord);
                    continue;
                }

                foreach (var record in Parts)
                {
                    if (record.EndDateTime < newRecord.EndDateTime &&
                        record.EndDateTime >= newRecord.StartDateTime &&
                        record.Type == newRecord.Type)
                    {
                        record.EndDateTime = newRecord.EndDateTime;
                        break;
                    }
                }
            }

            var temp = new List<Record>(Parts.ToArray());
            temp.Sort((x, y) => x.StartDateTime.CompareTo(y.StartDateTime));
            Parts.Clear();
            foreach (var record in temp)
                Parts.Enqueue(record);
        }

        protected void GetPartCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            Invalidate();

            if (OnGetPartCompleted != null)
                OnGetPartCompleted(this, null);
        }

        protected void GetPartAddFrontCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            Invalidate();

            if (_getPartToFrontQueue.Count <= 0)
            {
                if (OnGetPartCompleted != null)
                    OnGetPartCompleted(this, null);
                return;
            }

            if (_getPartAddFrontBackgroundWorker.IsBusy) return;

            GetPartArgument argument = _getPartToFrontQueue[0];
            _getPartToFrontQueue.Remove(argument);
            _getPartAddFrontBackgroundWorker.RunWorkerAsync(argument);
        }

        protected void GetDownloadPartCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            Invalidate();

            if (_camera == null) return;
            if (_getDownloadPartBackgroundWorker.IsBusy) return;

            var timer = new Timer { Interval = 3000 };
            timer.Tick += (s, e1) =>
            {
                UpdateDownloadPart();
                timer.Enabled = false;
                timer = null;
            };
            
            timer.Enabled = true;
            if (OnGetDownloadPartCompleted != null)
                OnGetDownloadPartCompleted(this, null);
        }

        protected void GetPartAddBackCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            Invalidate();

            if (_getPartToBackQueue.Count <= 0)
            {
                if (OnGetPartCompleted != null)
                    OnGetPartCompleted(this, null);
                return;
            }

            if (_getPartAddBackBackgroundWorker.IsBusy) return;

            GetPartArgument argument = _getPartToBackQueue[0];
            _getPartToBackQueue.Remove(argument);
            _getPartAddBackBackgroundWorker.RunWorkerAsync(argument);
        }

        //Draw Recording Data & Bookmark
        //private IEnumerable<Record> _visibleRecords;
        //private IEnumerable<Bookmark> _visibleBookmarks;

        protected void TrackerPaint(Object sender, PaintEventArgs e)
        {
            if (_camera == null) return;

            Graphics g = e.Graphics;

            Boolean hasStart = (TrackerContainer.RangeStartDate != DateTime.MinValue
                && TrackerContainer.RangeStartDate < TrackerContainer.VisibleMaxDateTime);
            //&& TrackerContainer.RangeStartDate > TrackerContainer.VisibleMinDateTime

            Boolean hasEnd = (TrackerContainer.RangeEndDate != DateTime.MaxValue
                && TrackerContainer.RangeEndDate > TrackerContainer.VisibleMinDateTime);
            //&& TrackerContainer.RangeEndDate < TrackerContainer.VisibleMaxDateTime 

            if (!hasStart && hasEnd)
            {
                if (TrackerContainer.RangeStartDate > TrackerContainer.VisibleMaxDateTime)
                    hasEnd = false;
            }

            if (hasStart && !hasEnd)
            {
                if (TrackerContainer.RangeEndDate < TrackerContainer.VisibleMinDateTime)
                    hasStart = false;
            }

            try
            {
                //_visibleRecords = _parts;
                var visibleRecords = (from record in Parts
                                      where
                                         record.StartDateTime < TrackerContainer.VisibleMaxDateTime &&
                                         record.EndDateTime > TrackerContainer.VisibleMinDateTime
                                      select record).ToList();
                //_visibleRecords = _parts;
                var visibleDownloadRecords = (from record in PlaybackParts
                                              where
                                                 record.StartDateTime < TrackerContainer.VisibleMaxDateTime &&
                                                 record.EndDateTime > TrackerContainer.VisibleMinDateTime
                                              select record);

                //paint sd record first
                var sortRecord = visibleRecords.Where(record => record.Type == EventType.SDRecord).ToList();

                //record paint first
                //normal record
                sortRecord.AddRange(visibleRecords.Where(record => record.Type == EventType.VideoRecord));

                if (Camera.CMS != null)
                {
                    //paint archive record first
                    if(Camera.CMS.Configure.WithArchiveServer)
                    {
                        sortRecord.AddRange(visibleRecords.Where(record => record.Type == EventType.ArchiveServerRecord));
                        Camera.ArchiveServerRecord.Clear();
                        Camera.ArchiveServerRecord.AddRange(visibleRecords.Where(record => record.Type == EventType.ArchiveServerRecord));
                    }
                }
                
                //download progress
                sortRecord.AddRange(visibleDownloadRecords);

                //event paint uppon on record
                sortRecord.AddRange(visibleRecords.Where(record => (record.Type != EventType.VideoRecord && record.Type != EventType.ArchiveServerRecord) ));

                visibleRecords = sortRecord;

                //Console.WriteLine("visibleRecords " + visibleRecords.Count() + " / " + _parts.Count);
                foreach (var record in visibleRecords)
                {
                    Int64 start;
                    Int64 width;

                    if (record.Type == EventType.VideoRecord)
                    {
                        if (TrackerContainer.RangeStartDate == TrackerContainer.RangeEndDate)
                        {
                            start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                            width = Math.Min(TrackerContainer.VisibleMaxDateTime.Ticks, record.EndDateTime.Ticks) - start;

                            record.PaintUnSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);

                            continue;
                        }

                        //R -> Record , H -> Highlight

                        if (hasStart && hasEnd)
                        {
                            //  |  HHH  |
                            if (TrackerContainer.RangeStartDate <= record.StartDateTime && TrackerContainer.RangeEndDate >= record.EndDateTime)
                            {
                                start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                                width = Math.Min(TrackerContainer.RangeEndDate.Ticks, record.EndDateTime.Ticks) - start;

                                record.PaintSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                    TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                                continue;
                            }
                        }

                        if (hasStart)
                        {
                            // RRRR|HHHH
                            if (record.StartDateTime < TrackerContainer.RangeStartDate && record.EndDateTime > TrackerContainer.RangeStartDate)
                            {
                                start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                                width = Math.Min(TrackerContainer.RangeStartDate.Ticks, record.EndDateTime.Ticks) - start;

                                record.PaintUnSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                    TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);

                                if (record.EndDateTime > TrackerContainer.RangeStartDate) //&& TrackerContainer.RangeStartDate > TrackerContainer.VisibleMinDateTime
                                {
                                    width = Math.Min(TrackerContainer.RangeEndDate.Ticks, record.EndDateTime.Ticks) - TrackerContainer.RangeStartDate.Ticks;

                                    record.PaintSelected(g, TrackerContainer.TicksToX(TrackerContainer.RangeStartDate.Ticks), TrackerContainer.PaintTop,
                                        TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);

                                    if (record.EndDateTime > TrackerContainer.RangeEndDate)
                                    {
                                        width = Math.Min(TrackerContainer.VisibleMaxDateTime.Ticks, record.EndDateTime.Ticks) - TrackerContainer.RangeEndDate.Ticks;

                                        record.PaintUnSelected(g, TrackerContainer.TicksToX(TrackerContainer.RangeEndDate.Ticks), TrackerContainer.PaintTop,
                                            TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                                    }
                                }

                                continue;
                            }

                            //RRR  |   HHHHH
                            if (!hasEnd && record.StartDateTime > TrackerContainer.RangeStartDate)
                            {
                                start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                                width = Math.Min(TrackerContainer.RangeEndDate.Ticks, record.EndDateTime.Ticks) - start;

                                record.PaintSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                    TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                                continue;
                            }
                        }

                        if (hasEnd)
                        {
                            // HHHH|RRRR
                            if (record.EndDateTime > TrackerContainer.RangeEndDate && record.StartDateTime < TrackerContainer.RangeEndDate)
                            {
                                if (record.StartDateTime < TrackerContainer.RangeEndDate)// && TrackerContainer.RangeEndDate < TrackerContainer.VisibleMaxDateTime
                                {
                                    start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                                    width = Math.Min(TrackerContainer.RangeEndDate.Ticks, record.EndDateTime.Ticks) - start;

                                    record.PaintSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                        TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                                }

                                start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.RangeEndDate.Ticks);
                                width = Math.Min(TrackerContainer.VisibleMaxDateTime.Ticks, record.EndDateTime.Ticks) - start;

                                record.PaintUnSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                    TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);

                                continue;
                            }

                            //HHHHH  |   RRR
                            if (!hasStart && record.EndDateTime < TrackerContainer.RangeEndDate)
                            {
                                start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                                width = Math.Min(TrackerContainer.RangeEndDate.Ticks, record.EndDateTime.Ticks) - start;

                                record.PaintSelected(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                    TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                                continue;
                            }
                        }
                    }

                    start = Math.Max(record.StartDateTime.Ticks, TrackerContainer.VisibleMinDateTime.Ticks);
                    width = Math.Min(TrackerContainer.VisibleMaxDateTime.Ticks, record.EndDateTime.Ticks) - start;

                    //record);
                    if (record.Type == EventType.VideoRecord || record.Type == EventType.UserDefine || record.Type == EventType.PlaybackDownload || record.Type == EventType.SDRecord || record.Type == EventType.ArchiveServerRecord)
                    {
                        record.Paint(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                            TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                    }
                    else //event
                    {
                        if (TrackerContainer.SearchEventType.Contains(record.Type))
                        {
                            record.Paint(g, TrackerContainer.TicksToX(start), TrackerContainer.PaintTop,
                                TrackerContainer.TicksToWidth(width), TrackerContainer.RecordHeight);
                        }
                    }

                    //Console.WriteLine(@"Full");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            PaintBookmark(g);
            //g.DrawLine(Pens.Red, Width / 2, 0, Width / 2, Height);
        }

        private void PaintBookmark(Graphics g)
        {
            if (_camera == null) return;

            //_visibleBookmarks = _camera.Bookmarks;
            var visibleBookmarks = (from bookmark in _camera.Bookmarks
                                    where
                                        bookmark.DateTime >= TrackerContainer.VisibleMinDateTime &&
                                        bookmark.DateTime <= TrackerContainer.VisibleMaxDateTime
                                    select bookmark);

            // Console.WriteLine("visibleBookmarks " + visibleBookmarks.Count() + " / " + _camera.Bookmarks.Count);
            //g.ScaleTransform(1.5, 1.0);
            foreach (var bookmark in visibleBookmarks)
            {
                bookmark.Selected = (bookmark.DateTime >= TrackerContainer.RangeStartDate &&
                                     bookmark.DateTime <= TrackerContainer.RangeEndDate);

                bookmark.Paint(g, TrackerContainer.TicksToX(bookmark.DateTime.Ticks), TrackerContainer.PaintTop);
            }
        }

        protected static String BookmarkChangedXml(String id)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Type", "Bookmark"));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Id", id));

            return xmlDoc.InnerXml;
        }

        public void EraserBookmark()
        {
            _camera.Bookmarks.Clear();

            var id = -1;
            if (_camera is IDeviceLayout)
            {
                foreach (var device in ((IDeviceLayout)_camera).Items)
                {
                    if (device == null) continue;
                    id = device.Id;
                    break;
                }
                if (id == -1)
                    return;
            }
            else if (_camera is ISubLayout)
            {
                var subLayout = _camera as ISubLayout;

                id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                if (id == -1)
                    return;
            }

            if (id == -1)
                id = _camera.Id;

            _camera.Server.Device.Save(BookmarkChangedXml(id.ToString()));
        }

        public void AddBookmark()
        {
            if (_camera.Bookmarks.Any(bookmark => bookmark.DateTime == TrackerContainer.DateTime))
            {
                return;
            }

            var newBookmark = new Bookmark
            {
                Creator = Server.Credential.UserName,
                DateTime = TrackerContainer.DateTime,
                CreateDateTime = DateTime.Now,
            };

            _camera.Bookmarks.Add(newBookmark);
            _camera.Bookmarks.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

            var id = -1;
            if (_camera is IDeviceLayout)
            {
                foreach (var device in ((IDeviceLayout)_camera).Items)
                {
                    if (device == null) continue;
                    id = device.Id;
                    break;
                }
                if (id == -1)
                    return;
            }
            else if (_camera is ISubLayout)
            {
                var subLayout = _camera as ISubLayout;

                id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                if (id == -1)
                    return;
            }

            if (id == -1)
                id = _camera.Id;

            _camera.Server.Device.Save(BookmarkChangedXml(id.ToString()));
        }

        public Int64 NextBookmarkTicks()
        {
            foreach (var bookmark in _camera.Bookmarks)
            {
                if (bookmark.DateTime > TrackerContainer.DateTime)
                    return bookmark.DateTime.Ticks;
            }

            return DateTime.MaxValue.Ticks;
        }

        public Int64 PreviousBookmarkTicks()
        {
            var descBookmarks = new List<Bookmark>(_camera.Bookmarks);
            descBookmarks.Reverse();
            foreach (Bookmark bookmark in descBookmarks)
            {
                if (bookmark.DateTime < TrackerContainer.DateTime)
                    return bookmark.DateTime.Ticks;
            }

            return DateTime.MinValue.Ticks;
        }

        public Int64 NextRecordTicks()
        {
            //if in cache has record data, jump in cache
            //foreach (Record record in _parts)
            //{
            //    if (record.StartDateTime > TrackerContainer.DateTime)
            //    {
            //        return record.StartDateTime.Ticks;
            //    }
            //}

            return DateTime.MaxValue.Ticks;
        }

        public Int64 NextRecordTicksViaCGI()
        {
            if (_camera == null) return DateTime.MaxValue.Ticks;

            UInt64 now = DateTimes.ToUtc(TrackerContainer.DateTime, Server.Server.TimeZone);
            if (Server.Server.TimeZone != _camera.Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(now);
                time += (Server.Server.TimeZone * 1000);
                time -= (_camera.Server.Server.TimeZone * 1000);
                now = Convert.ToUInt64(time);
            }
            return _camera.GetNextRecord(now).Ticks;
        }

        public Int64 PreviousRecordTicks()
        {
            //if in cache has record data, jump in cache
            //foreach (Record record in _reverseParts)
            //{
            //    if (record.EndDateTime < TrackerContainer.DateTime)
            //    {
            //        return record.EndDateTime.Ticks;
            //    }
            //}

            return DateTime.MinValue.Ticks;
        }

        public Int64 PreviousRecordTicksViaCGI()
        {
            if (_camera == null) return DateTime.MinValue.Ticks;

            UInt64 now = DateTimes.ToUtc(TrackerContainer.DateTime, Server.Server.TimeZone);
            if (Server.Server.TimeZone != _camera.Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(now);
                time += (Server.Server.TimeZone * 1000);
                time -= (_camera.Server.Server.TimeZone * 1000);
                now = Convert.ToUInt64(time);
            }

            return _camera.GetPreviousRecord(now).Ticks;
        }

        public Int64 BeginRecordTicksViaCGI()
        {
            if (_camera == null) return DateTime.MinValue.Ticks;

            return _camera.GetBeginRecord().Ticks;
        }

        private Int32 _mouseX;

        protected void TrackerMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= TrackerMouseMove;

            if (e.Button == MouseButtons.Right)
            {
                //_clearAllBookmark.Stop();
                Bookmark selectedBookmark = null;
                var point = new Point(e.X, e.Y);

                foreach (var bookmark in _camera.Bookmarks)
                {
                    if (bookmark.DateTime >= TrackerContainer.VisibleMinDateTime && bookmark.DateTime <= TrackerContainer.VisibleMaxDateTime)
                    {
                        if (bookmark.Contains(point) && bookmark.Contains(_removeBookmark))
                        {
                            selectedBookmark = bookmark;
                            break;
                        }
                    }
                }

                if (selectedBookmark != null)
                {
                    _camera.Bookmarks.Remove(selectedBookmark);
                    TrackerContainer.BookmarkRemoved(_camera);

                    var id = -1;
                    if (_camera is IDeviceLayout)
                    {
                        foreach (var device in ((IDeviceLayout)_camera).Items)
                        {
                            if (device == null) continue;
                            id = device.Id;
                            break;
                        }
                        if (id == -1)
                            return;
                    }
                    else if (_camera is ISubLayout)
                    {
                        var subLayout = _camera as ISubLayout;

                        id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                        if (id == -1)
                            return;
                    }

                    if (id == -1)
                        id = _camera.Id;

                    _camera.Server.Device.Save(BookmarkChangedXml(id.ToString()));

                    //clear all
                    /*if(_clearAllBookmark.ElapsedMilliseconds > 1500)
                    {
                        _camera.Bookmarks.Clear();
                        TrackerContainer.BookmarkRemoved(_camera);
                    }*/
                }
            }
        }

        private Point _removeBookmark = new Point(0, 0);

        protected void TrackerMouseDown(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //_clearAllBookmark.Reset();
                //_clearAllBookmark.Start();
                _removeBookmark.X = e.X;
                _removeBookmark.Y = e.Y;
                return;
            }

            _removeBookmark.X = 0;
            _removeBookmark.Y = 0;

            _mouseX = e.X;
            MouseMove -= TrackerMouseMove;
            MouseMove += TrackerMouseMove;
        }

        private void TrackerMouseMove(Object sender, MouseEventArgs e)
        {
            if (_mouseX != e.X)
            {
                MouseMove -= TrackerMouseMove;
                if (OnDragStart != null)
                    OnDragStart(this, new EventArgs<Int32>(e.X));
            }
        }

        private delegate Queue<Record> GetEventPartsDelegate(UInt16 part, UInt64 start, UInt64 end, EventType eventType);

        private EventType _eventType;
        private Boolean _loadingEvent;
        public void SearchEventAdd(EventType eventType)
        {
            UInt16 part = TrackerContainer.Parts;
            UInt64 startTime = TrackerContainer.StartTime;

            if (Server.Server.TimeZone != _camera.Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(startTime);
                time += (Server.Server.TimeZone * 1000);
                time -= (_camera.Server.Server.TimeZone * 1000);
                startTime = Convert.ToUInt64(time);
            }

            UInt64 endTime = startTime + TrackerContainer.PartsToTime(part);//TrackerContainer.EndTime

            _loadingEvent = true;
            _eventType = eventType;

            if (OnGetPartStart != null)
                OnGetPartStart(this, null);

            GetEventPartsDelegate getEventPartsDelegate = _camera.GetEventParts;
            getEventPartsDelegate.BeginInvoke(part, startTime, endTime, eventType, GetEventPartsCallback, getEventPartsDelegate);
        }

        private delegate void GetEventPartsCallbackDelegate(IAsyncResult result);
        private void GetEventPartsCallback(IAsyncResult result)
        {
            //stop by user
            if (!_loadingEvent)
            {
                if (OnGetPartCompleted != null)
                    OnGetPartCompleted(this, null);
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new GetEventPartsCallbackDelegate(GetEventPartsCallback), result);
                }
                catch (Exception)
                {
                }
                return;
            }

            var parts = ((GetEventPartsDelegate)result.AsyncState).EndInvoke(result);

            _loadingEvent = false;
            if (OnGetPartCompleted != null)
                OnGetPartCompleted(this, null);

            //after getevent back, user already remove this event, so no need show it.
            if (!TrackerContainer.SearchEventType.Contains(_eventType))
            {
                var keepRecord = new List<Record>(Parts);
                Parts.Clear();
                foreach (var record in keepRecord)
                {
                    if (record.Type == _eventType) continue;

                    Parts.Enqueue(record);
                }
                return;
            }

            if (parts.Count == 0) return;

            foreach (var eventRecord in parts)
                Parts.Enqueue(eventRecord);

            Invalidate();
        }

        public void SearchEventRemove(EventType eventType)
        {
            if (Parts.All(part => part.Type != eventType)) return;

            var keepRecord = new List<Record>(Parts);
            Parts.Clear();
            foreach (var record in keepRecord)
            {
                if (record.Type == eventType) continue;

                Parts.Enqueue(record);
            }

            Invalidate();
        }
    }
}
