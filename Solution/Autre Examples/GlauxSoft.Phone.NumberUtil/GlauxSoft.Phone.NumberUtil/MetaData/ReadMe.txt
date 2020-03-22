
PhoneNumberMetaData.compressed is a ZipStream compressed file from the PhoneNumberMetaData.xml.
This is due to minimize the size of the DLL, because the metadata is embedded into the assembly as a resource.
To build the compressed file, use the BuildHelper code and call method 'ZipIt()'.

Note: ZipStream is not compatible with WinZip. 