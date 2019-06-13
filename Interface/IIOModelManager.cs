using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constant;

namespace Interface
{
	public interface IIOEventHandle
	{
		String Name { get; set; }
		INVR NVR { get; set; }
		UInt16 Camera { get; set; }
	}

	public interface IIOModel
	{
		ReadyState ReadyState { get; set; }

		UInt16 Id { get; set; }
		String Name { get; set; }
		String Manufacture { get; set; }
		ServerCredential Credential { get; set; }

		UInt16 DICount { get; set; }
		UInt16 DOCount { get; set; }

		Dictionary<String, IIOEventHandle> Handles { get; set; }
	}

	public interface IIOModelManager : IDataManager
	{
		Dictionary<UInt16, IIOModel> IOModels { get; }
		UInt16 GetNewIOModelId();
	}
}
