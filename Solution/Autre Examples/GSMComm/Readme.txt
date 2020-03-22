GSMComm 1.21
Copyright (C) 2004-2011 Stefan Mayr


Contents of this file
---------------------

1) Overview
2) License
3) Features
4) Requirements
5) Deployment
6) History


1) Overview
-----------

GSMComm - the "GSM Communication Library" - is a set of components to aid developers in
performing SMS-related tasks with compatible GSM mobile phones.

Available on the web at: http://www.scampers.org/steve/sms/

Author: Stefan Mayr


2) License
----------

You can use this software in any personal or commercial projects for free, as long as the
files included are not modified in any way.

You can redistribute the installation package to other developers, as long as the package
is not modified in any way and is not sold.

You use this software at your own risk, the author is not responsible for any loss or
damage resulting from the use of this software.


3) Features
-----------

* Manage SMS messages: Send, read, delete, store, import and export messages,
  get memory status, get/set SMSC.
* Manage the phonebook: Create, find, delete, read, import and export phonebook entries,
  get memory status.
* Manage the phone: Read general info, reset configuration, enter PIN
* Supports notifications about new messages as well as forwarding them directly to the
  application.
* Detect phone connection/disconnection.
* Other SMS message variants also possible, such as alerts and notifications.
* Support for Smart Messaging and related messages to create, locate and recombine
  concatenated text messages, and also to create operator logo messages.
* Logging support with 4 levels to assist in troubleshooting.
* Support for GSM 7-bit default alphabet and Unicode for the message text.
* Supports some operator functions.
* Remoting support for sending SMS messages.
* SMS batch mode for faster sending of multiple messages.
* Execution of custom commands that are not directly supported.

Documentation is available in XML and CHM (HTML Help) formats and via source code samples.


4) Requirements
---------------

* Windows XP SP3, Server 2003 SP2, Vista SP1, Server 2008 or higher

* .NET Framework 2.0 SP2 or .NET Framework 4.0

* A mobile phone with a built-in GSM modem, or alternatively a dedicated GSM modem


5) Deployment
-------------

When used in a single application, the GSMComm components - the DLL files - can be copied
into the same directory as the application, this is sometimes referred to as
"xcopy-deployment".

If the library is shared among several applications, its components can be installed into
the Global Assembly Cache. This is possible with version 1.5 of the library and higher.

Do not use the installer for the GSMComm development package to deploy GSMComm to end users.


6) History
----------

Version 1.21:
-------------

Changes and fixes:

* Added a workaround for a communication issue, where you may receive an unexpected
  System.IOException with the error message "The I/O operation has been aborted because
  of either a thread exit or an application request." when using GSMComm to communicate
  with a phone for a longer amount of time (was observed after several days).

  The reason for this is a problem in the .NET SerialPort implementation. You can
  read a detailed explanation here:
  http://zachsaw.blogspot.com/2010/07/net-serialport-woes.html

  Thanks go to:
  - Rob Horvath, who did the research to determine that the .NET SerialPort problem is
    the reason for the issue in GSMComm, and who also did the verification that GSMComm
    works as expected with the workaround integrated.
  - Zach Saw for his detailed explanation and a ready-to-use workaround.

* The setup packages have been changed. There is now only one setup package for GSMComm
  for all supported operating system types.

* Updated the documentation to make clear that the GSMCommMain.SendMessage method updates
  the PDU with the message reference returned by the phone.


Version 1.20
------------

New features:

* Added methods to find and combine concatenated (long, multi-part) SMS messages.
  These methods are all present in the new class SmartMessageDecoder.

* Added methods to create, add and extract user data headers of SMS messages.
  For creating a user data header, the class SmartMessageFactory has a new method
  CreateUserDataHeader. The other new methods to add or get a user data header or data
  without any headers are in the SmsPdu class: AddUserDataHeader, GetUserDataHeader,
  GetUserDataWithoutHeader and GetUserDataTextWithoutHeader.

* Added a method to send messages to a specific destination port.
  The new method is CreatePortAddressHeader in the SmartMessageFactory class.
  This method creates a user data header that has to be added to a message.


Changes and fixes:

* Removed the smscAddress parameter on all methods in the SmartMessageFactory class, where
  the parameter was present. The methods affected are: CreateConcatTextMessage,
  CreateConcatUnicodeTextMessage, CreateOperatorLogoMessage.

* Replaced the CreateUnicodeTextMessage method with a new overload of the
  CreateConcatTextMessage method.

* Changed a few methods in the GsmPhone class that retrieve general information about the
  phone to use different AT commands. The methods were also renamed to reflect the change.
  The methods affected are:
  - RequestTAManufacturer: Uses now AT+CGMI instead of AT+GMI, renamed to RequestManufacturer.
  - RequestTAModel: Uses now AT+CGMM instead of AT+GMM, renamed to RequestModel.
  - RequestTARevision: Uses now AT+CGMR instead of AT+GMR, renamed to RequestRevision.
  - RequestTASerialNumber: Uses now AT+CGSN instead of AT+GSN, renamed to RequestSerialNumber.

  These changes also affect the IdentifyDevice method of the GSMCommMain class, which uses the
  above methods to obtain information.

* Fixed a bug that alphanumeric sender addresses can't be decoded.

* Fixed an issue that causes crashes due to unhandled exceptions in the communication thread
  if a USB-connected phone is unplugged (or the operating system decides to unplug it) while its
  COM port is in use and further communication is attempted before GSMComm determines that the
  phone is no longer connected.

  GSMComm addresses this issue by simulating that the device is still connected, but is no
  longer able to communicate: Attempts to send data succeed, and attempts to receive data
  return no data. If a response to a request is expected, this will usually cause exceptions
  because of a timeout.

* Fixed wrong decoding of the "More Messages To Send" flag in received SMS messages
  (SMS-DELIVER and SMS-STATUS-REPORT message types).

* Fixed a bug in the creation of concatenated Unicode text messages, where 6 bytes in the
  user data were unused per message part (used 133 instead of 139 possible bytes).


Setup changes:

The setup packages have been changed to contain both .NET 2.0 and .NET 4.0 versions of GSMComm.

With this change, also the directory structure of the installation changed. Exsting references
that point to the installation directory must be changed to the new location.

Here is a comparison of the old and new directory structure:

Old                                      New
<Program Files>\GSMComm for .NET 2.0     <Program Files>\GSMComm\DotNet20
<Program Files>\GSMComm for .NET 4.0     <Program Files>\GSMComm\DotNet40

The following rules are applied when updating from older GSMComm versions:
* Installed GSMComm versions that support the .NET Framework 4.0 ("GSMComm for .NET 4.0") are
  updated automatically when installing the new version.
* Other versions, like those that support the .NET Framework 2.0 ("GSMComm for .NET 2.0"), are
  not updated automatically and the new version is installed side-by-side.


Demo project changes:

* The sample code to send SMS messages has been enhanced to be less complex.

* The test string for concatenated messages now includes all printable GSM 7-bit characters.

* New sample code has been added to send an SMS message to a specific destination port.

* The behaviour of the "Create only" button has been changed. Creating a concatenated message
  using the "Create only" button now writes all created PDU parts into the output window.
  Previously, only the number of parts that would be sent, was shown in the output window.

* New sample code has been added to combine the parts of concatenated SMS messages.


Other sample project changes:

* Added two new sample projects "RemotingServer" and "RemotingClient".
  These projects show how to use GSMComm's built-in Remoting server, and shows an example
  how to use GSMComm within a Windows service.
  The RemotingServer is a Windows service, which must be installed before it can be used.
  The RemotingClient application connects to the RemotingServer to send SMS messages.


Version 1.15
------------

New features:

* Port names can now be used to connect to phones.
  In the communication classes GsmCommMain and GsmPhone both port names and port numbers
  can be used. In other areas like the SMS Server, only port names are supported. Checks
  whether the port actually exists are deferred to the operating system.

* Added the ability to execute custom AT commands.
  You can access the protocol level by calling the GetProtocol method in one of the
  communication classes. Background operations are suspended until ReleaseProtocol is
  called, which must be done as soon as execution of the custom commands is complete.

* Added log levels to the communication classes.
  Every log message received from GsmCommMain or GsmPhone now contains its associated
  level. There are 4 levels: Error, Warning, Info and Verbose. The "Verbose" level is
  used primarily for logging communications. The default log level is "Verbose" and can
  be changed with the LogLevel property.

* Added initial support for the .NET Framework 4.0.


Changes and fixes:

* GSMComm's .NET 2.0 version now requires .NET 2.0 SP2.
  On Windows Vista/Server 2008 and higher you might need to install .NET 3.5 SP1 instead.

* Removed .NET 1.1 support.
  Development packages for .NET 1.1 are no longer available.

* Setup packages now install for all users by default.

* Made changes to address threading issues in the internal logging facillity.
  These issues caused sudden NullReferenceExceptions, especially after running a long
  time and depending on the number of CPUs that are installed in the machine. It was not
  known to occur on single-CPU systems.

* Enhanced a few error messages.
  This is mainly about the unspecified "ERROR" response from the phone which can have
  many reasons. Since it typically denotes an unsupported command, an invalid parameter
  or an invalid state, the message text for the exception has been changed appropriately.

* Made the write storage return value optional when changing preferred message storages
  with the CPMS command.
  This is for compatibility to some phones that don't return this value.

* When reading single or multiple SMS messages, quoted alphabet values returned by the
  phone are now recognized.
  This prevented reading messages from phones that return the optional alphabet value,
  which resulted in 0 messages being decoded, although transferred correctly.

* Fixed a PDU decoder flaw that mis-calculated the length of an originating or
  destination address in some cases.
  In the error case, the whole remainder of the PDU was also interpreted as part of a
  phone number.

* Fixed formatting errors in concatenated SMS messages.
  This prevented several receiving phones from decoding concatenated messages correctly.

* Fixed decoding messages with no user data.


Version 1.11
------------
* Added a 64-bit installer.
* Fixed error that 8-bit characters do not transmit correctly between phone
  and application. The communication is now encoded with code page 1252
  (Windows). Previously 7-Bit ASCII has been used, which truncated the 8th
  bit. The .NET 1.1 version of GSMComm already uses CP 1252.
* Documentation has now VS2005 MSDN style (using one of the last NDoc 2.0
  alpha versions to keep content consistent).
* The first call to Open() now prints out the GSMComm version in the log.
* Selecting the active read or write storage now works on phones that return
  an alternative response that also contains the names of the active storages.
  This fixes also calls to methods that switch the storage before executing
  the actual command, like ReadMessages().
* Fixed an error that occurs when getting subscriber numbers and logging is
  enabled at the same time.
* Getting the subscriber numbers now returns also records that do not
  actually contain a number. Previously, such records were ignored.
* Fixed a parser error in situations where only the first character of an
  unsolicited message was received.
* Enhanced a few error messages.
* Enabling message notifications and routing is now possible on phones that only
  support the "ForwardAlways" indication mode.

Version 1.10
------------
* Fixed decoding of status report messages where additional data is present at the end.
* Fixed timestamp decoding of incoming messages.
* If checking incoming data for unsolicited messages fails, now a log entry is created.
  and the affected data is thrown away.
* Added sample code for exporting SMS messages as text in GSMComm Demo.
* Fixed a threading bug in communication.
* Added ToSortableString() to SmsTimestamp class for persisting timestamps, removed
  ToOriginalString() and ToEncodedString().

Version 1.9
-----------
New features:
* SMS batch mode for faster sending of multiple messages
* PIN code input

Other changes:
* Communication enhanced, now mostly using events instead of polling => faster
* Connection check delay can now be changed

Version 1.8
-----------
New features:
* First version to support the .NET Framework 2.0
* Remoting support for sending SMS messages
* Added function to get subscriber numbers (MSISDNs)

Other changes:
* Eliminated the RS232 component through the use of .NET's SerialPort class.
* Two new components GSMCommServer and GSMCommShared that carry the remoting code and the
  shared implementations.
* Setup refactoring:
    - All sample projects are now in one common 'Samples' directory.
    - Remnants of sample projects are now deleted properly on uninstall.
    - Sample projects now include the corresponding solution files.
    - Files that are only needed for setup and uninstall are no longer copied to the
      target machine.
* Fixed a threading issue when outputting text in the VB Events demo application.
* VB Events demo has now XP styles.

Version 1.7
-----------
New features:
* Can now get and set the SMSC (Service Center) address
* Can now list all network operators detected by the phone
* Can now get the currently selected operator & the selection mode (automatic/manual)
* Included new sample code to show how to consume GSMComm's events in VB.

Other changes:
* Introduced new global namespace "GsmComm". Since there is already a class called "GsmComm",
  the class was renamed to "GsmCommMain".
* Increased the timeout when waiting for a response from phone to 30 retries á 500ms delay
  (was 20 retries before)
* Demo aplication now has XP styles.
* Corrected versioning scheme. According to previous versions, this would be 1.70 now,
  but actually the version isn't that high yet. If you're directly updating from a version
  with such a high minor version number (1.31, 1.62, etc.) you'll get an error that
  a newer version of the product is already installed. In such a case uninstall the old
  version first and then install the new one.

Specific problems fixed:
* Enabling the message notification or message routing is now more flexible so that it
  works now with a broader range of phones.
  Thanks to Ali Ashgari and Petr Teplík for input on some Sony Ericsson, Nokia and
  Siemens phones.
* Some RS232 issues fixed. One of them caused the "PInvoke Stack Imbalance" that occurred
  when the "Open" method of the GsmComm class (now GsmCommMain) was called.
  Thanks to Petr Teplík for reporting the PInvoke issue.
* Eliminated the ObjectDisposedException in the demo application that occured sometimes
  when the form was closed.
* Fixed a threading issue when outputting text in the demo application.
  Thanks to Petr Teplík for reporting & quickly fixing it.

Version 1.62
------------
New features:
* Get signal quality and battery status from phone

Other changes:
* Fixed a bug decoding SMS submits, delivers and status reports with invalid address lengths.


Implementation details:

GsmComm/GsmPhone classes:
  New methods:
  * GetSignalQuality
  * GetBatteryCharge

New classes:
  * SignalQualityInfo
  * BatteryChargeInfo

SmsPdu/SmsSubmitPdu/SmsDeliverPdu/SmsStatusReportPdu classes:
  * Adapted for improved decoding of PDUs with invalid address lengths

Version 1.61
------------
New features:
* Unicode message text input and decoding now possible
* Unicode is also supported for sending long SMS messages

New features in GSMComm Demo:
* More SMS Send options: specify SMSC, send alert, request status report, send as Unicode
* Included sending of Smart Messaging compliant messages (long SMS and operator logos)
* Included text mode character set functions

Implementation details:

GsmPhone class:
* Exceptions during call to Open() are now rethrown instead of throwing a new exception
* GetMessageStorages(), SelectReadStorage(), SelectWriteStorage() now expect only two
  storage groups for modems that do not return a third group for preferred
  message storage.
* SelectCharacterSet() now validates the connection correctly

SmsPdu class:
* Unicode input and decoding now possible via the UserDataText property. Correct Data
  coding scheme must be set.

SmsSubmitPdu class:
* Additional constructor to specify also the data coding scheme.

SmartMessageFactory class:
* New function CreateConcatUnicodeTextMessage() to create long Unicode SMS messages

Version 1.6
-----------
General:
* Included demo program and its source, other source samples removed.
* Setup now removes previously installed versions of GSMComm if present.

New features:
* Read single SMS messages from the phone
* Explicitly decode a received short message, not only the ones read from the phone
* Write SMS messages to the phone without setting a specific status
* Receive notifications about new received messages
* Route new received messages directly to the application. Includes also support for
  optional acknowledement of the messages.
* Retrieve the memory status of the phone's storages.
* Reset the phone to its default configuration.
* Detect phone connection and disconnection.


Implementation details:

Component GSMCommunication:

General:
* New classes that deal with memory status and new message indications

GsmComm class:
  New methods:
  * ReadMessage
  * DecodeReceivedMessage
  * WriteRawMessageWithoutStatus
  * EnableMessageRouting, DisableMessageRouting
  * EnableMessageNotifications, DisableMessageNotifications
  * AcknowledgeNewMessage, RequireAcknowledge, IsAcknowledgeRequired
  * GetMessageMemoryStatus, GetPhonebookMemoryStatus
  * ResetToDefaultConfig

  New events:
  * MessageReceived, PhoneConnected, PhoneDisconnected

  Changed methods:
  * ReadMessages new returns a DecodedShortMessage array instead of ShortMessageFromPhone array
  * ReadRawMessages new returns a ShortMessageFromPhone array instead of ShortMessage array
  * DecodeShortMessage now expects a ShortMessageFromPhone parameter instead of ShortMessage

GsmPhone class:
  General:
  * Introduced a communication thread that allows checking for incoming data from the phone
    in the background. This "AutoPoll" function is currently only active when any notifications
    or routings are activated.
  * Nearly all functions were changed to wait a longer time before returning an error.
  * A periodic connection check is now performed to verify that there is a phone connected.
  * No phone functions can be executed as long as no phone was detected.
  * Most of the events are now fired asynchronously to prevent errors in communication flow.

  New methods:
  * ResetToDefaultConfig
  * SelectMessageService, GetCurrentMessageService
  * SetMessageIndications, GetMessageIndications
  * ReadMessage
  * AcknowledgeNewMessage
  * GetPhonebookMemoryStatus

  New events:
  * MessageReceived, PhoneConnected, PhoneDisconnected

  Changed methods:
  * GetManufacturer renamed to RequestTAManufacturer
  * GetModel renamed to RequestTAModel
  * GetRevision renamed to RequestTARevision
  * GetSerialNumber renamed to RequestTASerialNumber
  * GetPhonebook renamed to ReadPhonebookEntries
  * SelectReadStorage now returns a MemoryStatus
  * SelectWriteStorage now returns a MemoryStatus
  * ReadMessages renamed to ListMessages

ICommunication interface:
* ExecAndReceiveAnything for accepting any response the phone sends back

PhonebookEntry class:
* PhonebookEntry now a class again, since it is necessary to derive from it
* PhonebookEntryWithStorage is now derived from PhonebookEntry, no "Entry" property any longer

ShortMessage class:
* ShortMessage now a class again, since it is necessary to derive from it
* Moved Index and Status properties into derived class ShortMessageFromPhone
* Class formerly named ShortMessageFromPhone is now named DecodedShortMessage

Version 1.51
------------
General:
* GSMComm development download now available as Windows installer package

Component GSMCommunication:
* GsmComm has now a default constructor

Component PDUConverter:
* Bugfix for incorrect parsing of Message Flags => Status reports failed to decode

Version 1.5
-----------
All components:
* Assemblies are now signed so the can be put into the Global Assembly Cache

Component GSMCommunication:
* New DeleteFlag and DeleteScope enumerations defined
* NetworkErrorException class renamed to MessageServiceErrorException for clarity
* Made ShortMessage class a struct for easier serialization and because it's a data object anyway.
* Made PhonebookEntry and PhonebookEntryWithStorage classes also structs for easier serialisation.
* New Charset class that lists common character sets
GsmPhone class:
  * Now waiting longer when deleting messages, not just the single message timeout
    Reason: Timing problems with a Wavecom modem,
    reported by Martin Almström (martin@unibase.se)
  * It's now possible to use the <delflag> when deleting messages
  * An "ERROR" return is now considered a general communication error or syntax error
  * Introduced new message functions:
        GetMessageStorages, WriteMessageToMemory, SelectWriteStorage
  * Introduced charset functions:
        SelectCharacterSet, GetCurrentCharacterSet, GetSupportedCharacterSets
  * Introduced new phonebook functions:
        WritePhonebookEntry, DeletePhonebookEntry
  * Introduced events for monitoring receiving the data from the phone:
        ReceiveProgress and ReceiveCompleted
GsmComm class:
  * Introduced new message functions:
        DeleteMessages, GetMessageStorages, WriteRawMessage
  * Introduced new phonebook functions:
        GetPhonebookWithStorage, FindPhonebookEntriesWithStorage, CreatePhonebookEntry,
        DeletePhonebookEntry, DeleteAllPhonebookEntries
  * GetPhonebook function now returns just the PhonebookEntry array, no storage info any longer
  * FindPhonebookEntries function now returns just the PhonebookEntry array, no storage info any longer
  * Introduced charset functions:
        SelectCharacterSet, GetCurrentCharacterSet, GetSupportedCharacterSets
  * Introduced events for monitoring receiving the data from the phone:
        ReceiveProgress and ReceiveCompleted

Component PDUConverter:
* AddressType: New class to support decoding of the type-of-address octet,
  previously existing enumerator with same name replaced by public constants.
* DataCodingScheme class extended to support decoding of the DCS octet,
  kept previously existing enumerators and public constants.
* ProtocolID: Constants were changed to enumerators, new methods IsReserved() and IsSCSpecific()
* SmsPdu: Can now decode alphanumeric addresses.

Component RS232:
* Version number of RS232 assembly unchanged, although it was changed (signed), still version 1.16.

Version 1.4
-----------
Component PDUConverter:
* SmsPdu: Extended GetSafeText() to return also the reason for modifying the message.
* TextDataConverter: Extended StringTo7Bit() to return also the info if invalid characters
  were replaced during the conversion.

Version 1.32
------------
All components:
* General source code documentation update

Version 1.31
------------
Component GSMCommunication:
GsmPhone class:
  * Introduced "DeleteMessage()"
  * Made low level interface ICommunication public (explicit interface)
GsmComm class:
  * Introduced "DeleteMessage()"
  * Added "Index" property to ShortMessageFromPhone

Component PDUConverter:
* TextDataConverter: Bugfix for incorrectly converted ampersand character (&),
  thanks to Danuh Adipura (monyetsoft@gmail.com)

Version 1.3
-----------
Component GSMCommunication:
GsmPhone class:
  * Now throwing an exception upon call to "Open()" if the specified port can't be opened
  * Introduced "IsOpen()"
GsmComm class:
  * Introduced "ReadRawMessages()"
  * Introduced "IsOpen()"

Component PDUConverter:
* TextDataConverter: Now returning position instead of index in exception messages for the 7Bit converter
SmsPdu class:
  * Renamed "GetSafe7BitText" to "GetSafeText"
  * Introduced "GetMaxTextLength()" and "GetTextLength()"

Version 1.2
-----------
Component GSMCommunication:
GsmComm class:
  * SendMessage() now throws an exception upon an error
    and also fires ok and error events.
  * MessageEventArgs now contains the exception that caused the error
  * Some little text changes
  * SendMessages() does not catch exceptions any longer
GsmPhone class:
  * Now validating some of the answers via regular expressions
    (error codes, message reference)
  * Improved error messages in exceptions, moved commTrace to a separate property.
  * IsConnected() now really talks to the phone to see if it's there
  * Removed the "connected" field, too unreliable for a connection check

Component PDUConverter:
SmsPdu class:
  * GetActualLength() replaced by ActualLength
  * GetTotalLength() replaced by TotalLength
  * Added GetSafe7BitText() method
TextDataConverter class:
  * Some little text changes
  * Built in a workaround for the "@" bug in the decoder,
    so only @ signs get now stripped when string length is multiple of 8.
    Safe for most users.
* RelativeValidityPeriod: Duration now written as text, such as "24h" instead of string representation of TimeSpan.
* SmartMessageFactory: Included a constant for an "empty" operator logo

Version 1.0
-----------
Initial release
