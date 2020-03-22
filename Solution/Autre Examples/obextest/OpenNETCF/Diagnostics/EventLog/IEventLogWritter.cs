using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Defines the interface that will be used by EventLog to write to a log
	/// </summary>
	public interface IEventLogWritter
	{
		event EntryWrittenEventHandler EntryWritten;
		event EventHandler EventLogCollectionUpdated;

		string Source{get;set;}
		string Log{get;set;}
		string LogDisplayName{get;set;}
		string LogFileName{get;}
		string LogPath{get;}

		void Delete(string logName);
		void Clear();
		void Close();
		EventLog[] GetEventLogs();
		EventLogEntryCollection Entries{get;}
		bool Exists(string logName);
		
		void WriteEntry(string message);
		void WriteEntry(string message, EventLogEntryType type);
		void WriteEntry(string source, string message);
		void WriteEntry(string message, EventLogEntryType type, int eventID);
		void WriteEntry(string source, string message, EventLogEntryType type);
		void WriteEntry(string message, EventLogEntryType type, int eventID, short category);
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID);
		void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData);
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category);
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData);
		
	}
}
