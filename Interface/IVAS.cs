namespace Interface
{
    public interface IVAS : IServer
    {
        INVRManager NVR { get; }

        void NVRModify(INVR nvr);
    }
}