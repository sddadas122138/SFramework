
using System.Collections;
using System.Collections.Generic;
using System;
using Cfg.H;
using Cfg.Cm;

namespace Cfg.G
{
	public partial class CAllPlayer : TBaseConfigEx, ICfg
	{
		public int Id{ get { return m_Id; } private set { m_Id = value; } } 
		private int m_Id;
		public string MagicPath{ get { if (LineStr != null) { AfterInitialize(); } return m_MagicPath; } private set { m_MagicPath = value; } } 
		private string m_MagicPath;
		public int Color{ get { if (LineStr != null) { AfterInitialize(); } return m_Color; } private set { m_Color = value; } } 
		private int m_Color;
		private int Idx;
		private string LineStr;
		public void PreInitialize(int idx, string line)
		{
			Idx = idx;
			Id = CParse.ReadIntegerAlt(line, 0, line.Length);
			Id = Id;
			LineStr = line;
		}
		public void AfterInitialize(){ Initialize(Idx, LineStr); LineStr = null; }
		public void Initialize(int idx, string line)
		{
			var datas = line.Split(CParse.DataDelimiters);
			m_MagicPath = CParse.ReadString(datas[1]);
			m_Color = CParse.ReadInteger(datas[2]);
		}
		private Dictionary<string, object> _Kvs;
		public Dictionary<string, object> Kvs { get { if(_Kvs == null) { InitKvs(); } return _Kvs; } }
		private void InitKvs()
		{
			_Kvs = new Dictionary<string, object>();
			var kvs = _Kvs;
			kvs["id"] = m_Id;
			kvs["magic_path"] = m_MagicPath;
			kvs["color"] = m_Color;
		}
		object ICfg.Get(string key)
		{
			if (LineStr != null) AfterInitialize();
			return Kvs.ContainsKey(key) ? Kvs[key] : null;
		}
	}
	public partial class CKnowledge : TBaseConfigEx, ICfg
	{
		public int Id{ get { return m_Id; } private set { m_Id = value; } } 
		private int m_Id;
		public string Desc{ get { if (LineStr != null) { AfterInitialize(); } return m_Desc; } private set { m_Desc = value; } } 
		private string m_Desc;
		public string OptionOne{ get { if (LineStr != null) { AfterInitialize(); } return m_OptionOne; } private set { m_OptionOne = value; } } 
		private string m_OptionOne;
		public string OptionTwo{ get { if (LineStr != null) { AfterInitialize(); } return m_OptionTwo; } private set { m_OptionTwo = value; } } 
		private string m_OptionTwo;
		public string OptionThree{ get { if (LineStr != null) { AfterInitialize(); } return m_OptionThree; } private set { m_OptionThree = value; } } 
		private string m_OptionThree;
		public string OptionFour{ get { if (LineStr != null) { AfterInitialize(); } return m_OptionFour; } private set { m_OptionFour = value; } } 
		private string m_OptionFour;
		private int Idx;
		private string LineStr;
		public void PreInitialize(int idx, string line)
		{
			Idx = idx;
			Id = CParse.ReadIntegerAlt(line, 0, line.Length);
			Id = Id;
			LineStr = line;
		}
		public void AfterInitialize(){ Initialize(Idx, LineStr); LineStr = null; }
		public void Initialize(int idx, string line)
		{
			var datas = line.Split(CParse.DataDelimiters);
			m_Desc = CParse.ReadString(datas[1]);
			m_OptionOne = CParse.ReadString(datas[2]);
			m_OptionTwo = CParse.ReadString(datas[3]);
			m_OptionThree = CParse.ReadString(datas[4]);
			m_OptionFour = CParse.ReadString(datas[5]);
		}
		private Dictionary<string, object> _Kvs;
		public Dictionary<string, object> Kvs { get { if(_Kvs == null) { InitKvs(); } return _Kvs; } }
		private void InitKvs()
		{
			_Kvs = new Dictionary<string, object>();
			var kvs = _Kvs;
			kvs["id"] = m_Id;
			kvs["desc"] = m_Desc;
			kvs["option_one"] = m_OptionOne;
			kvs["option_two"] = m_OptionTwo;
			kvs["option_three"] = m_OptionThree;
			kvs["option_four"] = m_OptionFour;
		}
		object ICfg.Get(string key)
		{
			if (LineStr != null) AfterInitialize();
			return Kvs.ContainsKey(key) ? Kvs[key] : null;
		}
	}
	public partial class CMagic : TBaseConfigEx, ICfg
	{
		public int Id{ get { return m_Id; } private set { m_Id = value; } } 
		private int m_Id;
		public string MagicPath{ get { if (LineStr != null) { AfterInitialize(); } return m_MagicPath; } private set { m_MagicPath = value; } } 
		private string m_MagicPath;
		private int Idx;
		private string LineStr;
		public void PreInitialize(int idx, string line)
		{
			Idx = idx;
			Id = CParse.ReadIntegerAlt(line, 0, line.Length);
			Id = Id;
			LineStr = line;
		}
		public void AfterInitialize(){ Initialize(Idx, LineStr); LineStr = null; }
		public void Initialize(int idx, string line)
		{
			var datas = line.Split(CParse.DataDelimiters);
			m_MagicPath = CParse.ReadString(datas[1]);
		}
		private Dictionary<string, object> _Kvs;
		public Dictionary<string, object> Kvs { get { if(_Kvs == null) { InitKvs(); } return _Kvs; } }
		private void InitKvs()
		{
			_Kvs = new Dictionary<string, object>();
			var kvs = _Kvs;
			kvs["id"] = m_Id;
			kvs["magic_path"] = m_MagicPath;
		}
		object ICfg.Get(string key)
		{
			if (LineStr != null) AfterInitialize();
			return Kvs.ContainsKey(key) ? Kvs[key] : null;
		}
	}
	public partial class CProp : TBaseConfigEx, ICfg
	{
		public int Id{ get { return m_Id; } private set { m_Id = value; } } 
		private int m_Id;
		public string MagicPath{ get { if (LineStr != null) { AfterInitialize(); } return m_MagicPath; } private set { m_MagicPath = value; } } 
		private string m_MagicPath;
		public int Color{ get { if (LineStr != null) { AfterInitialize(); } return m_Color; } private set { m_Color = value; } } 
		private int m_Color;
		private int Idx;
		private string LineStr;
		public void PreInitialize(int idx, string line)
		{
			Idx = idx;
			Id = CParse.ReadIntegerAlt(line, 0, line.Length);
			Id = Id;
			LineStr = line;
		}
		public void AfterInitialize(){ Initialize(Idx, LineStr); LineStr = null; }
		public void Initialize(int idx, string line)
		{
			var datas = line.Split(CParse.DataDelimiters);
			m_MagicPath = CParse.ReadString(datas[1]);
			m_Color = CParse.ReadInteger(datas[2]);
		}
		private Dictionary<string, object> _Kvs;
		public Dictionary<string, object> Kvs { get { if(_Kvs == null) { InitKvs(); } return _Kvs; } }
		private void InitKvs()
		{
			_Kvs = new Dictionary<string, object>();
			var kvs = _Kvs;
			kvs["id"] = m_Id;
			kvs["magic_path"] = m_MagicPath;
			kvs["color"] = m_Color;
		}
		object ICfg.Get(string key)
		{
			if (LineStr != null) AfterInitialize();
			return Kvs.ContainsKey(key) ? Kvs[key] : null;
		}
	}
	public partial class CTFJLRecruit : TBaseConfigEx, ICfg
	{
		public int Id{ get { return m_Id; } private set { m_Id = value; } } 
		private int m_Id;
		public List<string> Weight{ get { if (LineStr != null) { AfterInitialize(); } return m_Weight; } private set { m_Weight = value; } } 
		private List<string> m_Weight;
		public List<string> DebrisCount{ get { if (LineStr != null) { AfterInitialize(); } return m_DebrisCount; } private set { m_DebrisCount = value; } } 
		private List<string> m_DebrisCount;
		public string Sss{ get { if (LineStr != null) { AfterInitialize(); } return m_Sss; } private set { m_Sss = value; } } 
		private string m_Sss;
		public string Aaa{ get { if (LineStr != null) { AfterInitialize(); } return m_Aaa; } private set { m_Aaa = value; } } 
		private string m_Aaa;
		public string Sadasd{ get { if (LineStr != null) { AfterInitialize(); } return m_Sadasd; } private set { m_Sadasd = value; } } 
		private string m_Sadasd;
		public string Tsss{ get { if (LineStr != null) { AfterInitialize(); } return m_Tsss; } private set { m_Tsss = value; } } 
		private string m_Tsss;
		private int Idx;
		private string LineStr;
		public void PreInitialize(int idx, string line)
		{
			Idx = idx;
			Id = CParse.ReadIntegerAlt(line, 0, line.Length);
			Id = Id;
			LineStr = line;
		}
		public void AfterInitialize(){ Initialize(Idx, LineStr); LineStr = null; }
		public void Initialize(int idx, string line)
		{
			var datas = line.Split(CParse.DataDelimiters);
			m_Weight = CParse.ReadArrString(datas[1]);
			m_DebrisCount = CParse.ReadArrString(datas[2]);
			m_Sss = CParse.ReadString(datas[3]);
			m_Aaa = CParse.ReadString(datas[4]);
			m_Sadasd = CParse.ReadString(datas[5]);
			m_Tsss = CParse.ReadString(datas[6]);
		}
		private Dictionary<string, object> _Kvs;
		public Dictionary<string, object> Kvs { get { if(_Kvs == null) { InitKvs(); } return _Kvs; } }
		private void InitKvs()
		{
			_Kvs = new Dictionary<string, object>();
			var kvs = _Kvs;
			kvs["id"] = m_Id;
			kvs["weight"] = m_Weight;
			kvs["debris_count"] = m_DebrisCount;
			kvs["sss"] = m_Sss;
			kvs["aaa"] = m_Aaa;
			kvs["sadasd"] = m_Sadasd;
			kvs["tsss"] = m_Tsss;
		}
		object ICfg.Get(string key)
		{
			if (LineStr != null) AfterInitialize();
			return Kvs.ContainsKey(key) ? Kvs[key] : null;
		}
	}
}
namespace Cfg.H
{
	public partial class CTypeName
	{
		static CTypeName()
		{
			Tns = new Dictionary<Type, string>()
			{
				{typeof(Cfg.G.CAllPlayer),"Assets/Config/Resources/AllPlayerCfg.txt"},
				{typeof(Cfg.G.CKnowledge),"Assets/Config/Resources/KnowledgeCfg.txt"},
				{typeof(Cfg.G.CMagic),"Assets/Config/Resources/MagicCfg.txt"},
				{typeof(Cfg.G.CProp),"Assets/Config/Resources/PropCfg.txt"},
				{typeof(Cfg.G.CTFJLRecruit),"Assets/Config/Resources/TFJLRecruitCfg.txt"},
			};
		}
	}
}

namespace Cfg.C
{
	public partial class CAllPlayer : CH<Cfg.G.CAllPlayer>
	{
	}
	public partial class CKnowledge : CH<Cfg.G.CKnowledge>
	{
	}
	public partial class CMagic : CH<Cfg.G.CMagic>
	{
	}
	public partial class CProp : CH<Cfg.G.CProp>
	{
	}
	public partial class CTFJLRecruit : CH<Cfg.G.CTFJLRecruit>
	{
	}
}