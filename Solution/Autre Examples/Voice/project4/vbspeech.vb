Option Strict Off
Option Explicit On
Module VBSPEECH
	Public Const SVFN_LEN As Short = (262)
	Public Const LANG_LEN As Short = (64)
	Public Const EI_TITLESIZE As Short = (128)
	Public Const EI_DESCSIZE As Short = (512)
	Public Const EI_FIXSIZE As Short = (512)
	Public Const SVPI_MFGLEN As Short = (64)
	Public Const SVPI_PRODLEN As Short = (64)
	Public Const SVPI_COMPLEN As Short = (64)
	Public Const SVPI_COPYRIGHTLEN As Short = (128)
	Public Const SVI_MFGLEN As Short = (SVPI_MFGLEN)
	
	Public Const AUDERR_NONE As Short = &H0s
	Public Const AUDERR_BADDEVICEID As Integer = &H80040301
	Public Const AUDERR_NEEDWAVEFORMAT As Integer = &H80040302
	Public Const AUDERR_NOTSUPPORTED As Integer = &H80004001
	Public Const AUDERR_NOTENOUGHDATA As Integer = &H80040201
	Public Const AUDERR_NOTPLAYING As Integer = &H80040306
	Public Const AUDERR_INVALIDPARAM As Integer = &H80070057
	Public Const AUDERR_WAVEFORMATNOTSUPPORTED As Integer = &H80040202
	Public Const AUDERR_WAVEDEVICEBUSY As Integer = &H80040203
	Public Const AUDERR_WAVEDEVNOTSUPPORTED As Integer = &H80040312
	Public Const AUDERR_NOTRECORDING As Integer = &H80040313
	Public Const AUDERR_INVALIDFLAG As Integer = &H80040204
	Public Const AUDERR_INVALIDHANDLE As Integer = &H80070006
	Public Const AUDERR_NODRIVER As Integer = &H80040317
	Public Const AUDERR_HANDLEBUSY As Integer = &H80040318
	Public Const AUDERR_INVALIDNOTIFYSINK As Integer = &H80040319
	Public Const AUDERR_WAVENOTENABLED As Integer = &H8004031A
	Public Const AUDERR_ALREADYCLAIMED As Integer = &H8004031D
	Public Const AUDERR_NOTCLAIMED As Integer = &H8004031E
	Public Const AUDERR_STILLPLAYING As Integer = &H8004031F
	Public Const AUDERR_ALREADYSTARTED As Integer = &H80040320
	Public Const AUDERR_SYNCNOTALLOWED As Integer = &H80040321
	
	Public Const SRERR_NONE As Short = &H0s
	Public Const SRERR_OUTOFDISK As Integer = &H80040205
	Public Const SRERR_NOTSUPPORTED As Integer = &H80004001
	Public Const SRERR_NOTENOUGHDATA As Integer = &H80040201
	Public Const SRERR_VALUEOUTOFRANGE As Integer = &H8000FFFF
	Public Const SRERR_GRAMMARTOOCOMPLEX As Integer = &H80040406
	Public Const SRERR_GRAMMARWRONGTYPE As Integer = &H80040407
	Public Const SRERR_INVALIDWINDOW As Integer = &H8004000F
	Public Const SRERR_INVALIDPARAM As Integer = &H80070057
	Public Const SRERR_INVALIDMODE As Integer = &H80040206
	Public Const SRERR_TOOMANYGRAMMARS As Integer = &H8004040B
	Public Const SRERR_INVALIDLIST As Integer = &H80040207
	Public Const SRERR_WAVEDEVICEBUSY As Integer = &H80040203
	Public Const SRERR_WAVEFORMATNOTSUPPORTED As Integer = &H80040202
	Public Const SRERR_INVALIDCHAR As Integer = &H80040208
	Public Const SRERR_GRAMTOOCOMPLEX As Integer = &H80040406
	Public Const SRERR_GRAMTOOLARGE As Integer = &H80040411
	Public Const SRERR_INVALIDINTERFACE As Integer = &H80004002
	Public Const SRERR_INVALIDKEY As Integer = &H80040209
	Public Const SRERR_INVALIDFLAG As Integer = &H80040204
	Public Const SRERR_GRAMMARERROR As Integer = &H80040416
	Public Const SRERR_INVALIDRULE As Integer = &H80040417
	Public Const SRERR_RULEALREADYACTIVE As Integer = &H80040418
	Public Const SRERR_RULENOTACTIVE As Integer = &H80040419
	Public Const SRERR_NOUSERSELECTED As Integer = &H8004041A
	Public Const SRERR_BAD_PRONUNCIATION As Integer = &H8004041B
	Public Const SRERR_DATAFILEERROR As Integer = &H8004041C
	Public Const SRERR_GRAMMARALREADYACTIVE As Integer = &H8004041D
	Public Const SRERR_GRAMMARNOTACTIVE As Integer = &H4041E
	Public Const SRERR_GLOBALGRAMMARALREADYACTIVE As Integer = &H8004041F
	Public Const SRERR_LANGUAGEMISMATCH As Integer = &H80040420
	Public Const SRERR_MULTIPLELANG As Integer = &H80040421
	Public Const SRERR_LDGRAMMARNOWORDS As Integer = &H80040422
	Public Const SRERR_NOLEXICON As Integer = &H80040423
	Public Const SRERR_SPEAKEREXISTS As Integer = &H80040424
	Public Const SRERR_GRAMMARENGINEMISMATCH As Integer = &H80040425
	Public Const SRERR_BOOKMARKEXISTS As Integer = &H80040426
	Public Const SRERR_BOOKMARKDOESNOTEXIST As Integer = &H80040427
	Public Const SRERR_MICWIZARDCANCELED As Integer = &H80040428
	Public Const SRERR_WORDTOOLONG As Integer = &H80040429
	Public Const SRERR_BAD_WORD As Integer = &H8004042A
	Public Const E_WRONGTYPE As Integer = &H8004020C
	Public Const E_BUFFERTOOSMALL As Integer = &H8004020D
	
	Public Const TTSERR_NONE As Short = &H0s
	Public Const TTSERR_INVALIDINTERFACE As Integer = &H80004002
	Public Const TTSERR_OUTOFDISK As Integer = &H80040205
	Public Const TTSERR_NOTSUPPORTED As Integer = &H80004001
	Public Const TTSERR_VALUEOUTOFRANGE As Integer = &H8000FFFF
	Public Const TTSERR_INVALIDWINDOW As Integer = &H8004000F
	Public Const TTSERR_INVALIDPARAM As Integer = &H80070057
	Public Const TTSERR_INVALIDMODE As Integer = &H80040206
	Public Const TTSERR_INVALIDKEY As Integer = &H80040209
	Public Const TTSERR_WAVEFORMATNOTSUPPORTED As Integer = &H80040202
	Public Const TTSERR_INVALIDCHAR As Integer = &H80040208
	Public Const TTSERR_QUEUEFULL As Integer = &H8004020A
	Public Const TTSERR_WAVEDEVICEBUSY As Integer = &H80040203
	Public Const TTSERR_NOTPAUSED As Integer = &H80040501
	Public Const TTSERR_ALREADYPAUSED As Integer = &H80040502
	
	Public Const VCMDERR_NONE As Short = &H0s
	Public Const VCMDERR_OUTOFMEM As Integer = &H8007000E
	Public Const VCMDERR_OUTOFDISK As Integer = &H80040205
	Public Const VCMDERR_NOTSUPPORTED As Integer = &H80004001
	Public Const VCMDERR_VALUEOUTOFRANGE As Integer = &H8000FFFF
	Public Const VCMDERR_MENUTOOCOMPLEX As Integer = &H80040606
	Public Const VCMDERR_MENUWRONGLANGUAGE As Integer = &H80040607
	Public Const VCMDERR_INVALIDWINDOW As Integer = &H8004000F
	Public Const VCMDERR_INVALIDPARAM As Integer = &H80070057
	Public Const VCMDERR_INVALIDMODE As Integer = &H80040206
	Public Const VCMDERR_TOOMANYMENUS As Integer = &H8004060
	Public Const VCMDERR_INVALIDLIST As Integer = &H80040207
	Public Const VCMDERR_MENUDOESNOTEXIST As Integer = &H8004060D
	Public Const VCMDERR_MENUACTIVE As Integer = &H8004060E
	Public Const VCMDERR_NOENGINE As Integer = &H8004060F
	Public Const VCMDERR_NOGRAMMARINTERFACE As Integer = &H80040610
	Public Const VCMDERR_NOFINDINTERFACE As Integer = &H80040611
	Public Const VCMDERR_CANTCREATESRENUM As Integer = &H80040612
	Public Const VCMDERR_NOSITEINFO As Integer = &H80040613
	Public Const VCMDERR_SRFINDFAILED As Integer = &H80040614
	Public Const VCMDERR_CANTCREATEAUDIODEVICE As Integer = &H80040615
	Public Const VCMDERR_CANTSETDEVICE As Integer = &H80040616
	Public Const VCMDERR_CANTSELECTENGINE As Integer = &H80040617
	Public Const VCMDERR_CANTCREATENOTIFY As Integer = &H80040618
	Public Const VCMDERR_CANTCREATEDATASTRUCTURES As Integer = &H80040619
	Public Const VCMDERR_CANTINITDATASTRUCTURES As Integer = &H8004061A
	Public Const VCMDERR_NOCACHEDATA As Integer = &H8004061B
	Public Const VCMDERR_NOCOMMANDS As Integer = &H8004061C
	Public Const VCMDERR_CANTXTRACTWORDS As Integer = &H8004061D
	Public Const VCMDERR_CANTGETDBNAME As Integer = &H8004061E
	Public Const VCMDERR_CANTCREATEKEY As Integer = &H8004061F
	Public Const VCMDERR_CANTCREATEDBNAME As Integer = &H80040620
	Public Const VCMDERR_CANTUPDATEREGISTRY As Integer = &H80040621
	Public Const VCMDERR_CANTOPENREGISTRY As Integer = &H80040622
	Public Const VCMDERR_CANTOPENDATABASE As Integer = &H80040623
	Public Const VCMDERR_CANTCREATESTORAGE As Integer = &H80040624
	Public Const VCMDERR_CANNOTMIMIC As Integer = &H80040625
	Public Const VCMDERR_MENUEXIST As Integer = &H80040626
	Public Const VCMDERR_MENUOPEN As Integer = &H80040627
	Public Const VTXTERR_NONE As Short = &H0s
	Public Const VTXTERR_OUTOFMEM As Integer = &H8007000E
	Public Const VTXTERR_EMPTYSPEAKSTRING As Integer = &H8004020B
	Public Const VTXTERR_INVALIDPARAM As Integer = &H80070057
	Public Const VTXTERR_INVALIDMODE As Integer = &H80040206
	Public Const VTXTERR_NOENGINE As Integer = &H8004070F
	Public Const VTXTERR_NOFINDINTERFACE As Integer = &H80040711
	Public Const VTXTERR_CANTCREATETTSENUM As Integer = &H80040712
	Public Const VTXTERR_NOSITEINFO As Integer = &H80040713
	Public Const VTXTERR_TTSFINDFAILED As Integer = &H80040714
	Public Const VTXTERR_CANTCREATEAUDIODEVICE As Integer = &H80040715
	Public Const VTXTERR_CANTSETDEVICE As Integer = &H80040716
	Public Const VTXTERR_CANTSELECTENGINE As Integer = &H80040717
	Public Const VTXTERR_CANTCREATENOTIFY As Integer = &H80040718
	Public Const VTXTERR_NOTENABLED As Integer = &H80040719
	Public Const VTXTERR_OUTOFDISK As Integer = &H80040205
	Public Const VTXTERR_NOTSUPPORTED As Integer = &H80004001
	Public Const VTXTERR_NOTENOUGHDATA As Integer = &H80040201
	Public Const VTXTERR_QUEUEFULL As Integer = &H8004020A
	Public Const VTXTERR_VALUEOUTOFRANGE As Integer = &H8000FFFF
	Public Const VTXTERR_INVALIDWINDOW As Integer = &H8004000F
	Public Const VTXTERR_WAVEDEVICEBUSY As Integer = &H80040203
	Public Const VTXTERR_WAVEFORMATNOTSUPPORTED As Integer = &H80040202
	Public Const VTXTERR_INVALIDCHAR As Integer = &H80040208
	Public Const LEXERR_INVALIDTEXTCHAR As Integer = &H80040801
	Public Const LEXERR_INVALIDSENSE As Integer = &H80040802
	Public Const LEXERR_NOTINLEX As Integer = &H80040803
	Public Const LEXERR_OUTOFDISK As Integer = &H80040804
	Public Const LEXERR_INVALIDPRONCHAR As Integer = &H80040805
	Public Const LEXERR_ALREADYINLEX As Integer = &H40806
	Public Const LEXERR_PRNBUFTOOSMALL As Integer = &H80040807
	Public Const LEXERR_ENGBUFTOOSMALL As Integer = &H80040808
	Public Const LEXERR_INVALIDLEX As Integer = &H80040809
	
	Public Const CHARSET_TEXT As Short = 0
	Public Const CHARSET_IPAPHONETIC As Short = 1
	Public Const CHARSET_ENGINEPHONETIC As Short = 2
	Public Const VPS_UNKNOWN As Short = 0
	Public Const VPS_NOUN As Short = 1
	Public Const VPS_VERB As Short = 2
	Public Const VPS_ADVERB As Short = 3
	Public Const VPS_ADJECTIVE As Short = 4
	Public Const VPS_PROPERNOUN As Short = 5
	Public Const VPS_PRONOUN As Short = 6
	Public Const VPS_CONJUNCTION As Short = 7
	Public Const VPS_CARDINAL As Short = 8
	Public Const VPS_ORDINAL As Short = 9
	Public Const VPS_DETERMINER As Short = 10
	Public Const VPS_QUANTIFIER As Short = 11
	Public Const VPS_PUNCTUATION As Short = 12
	Public Const VPS_CONTRACTION As Short = 13
	Public Const VPS_INTERJECTION As Short = 14
	Public Const VPS_ABBREVIATION As Short = 15
	Public Const VPS_PREPOSITION As Short = 16
	
	Public Const TTSBASEATTR As Short = &H1000s
	Public Const SRBASEATTR As Short = &H2000s
	Public Const VDCTBASEATTR As Short = &H3000s
	Public Const VCMDBASEATTR As Short = &H4000s
	Public Const VTXTBASEATTR As Short = &H5000s
	Public Const AUDBASEATTR As Short = &H6000s
	Public Const TTSATTR_PITCH As Short = (1)
	Public Const TTSATTR_REALTIME As Short = (0)
	Public Const TTSATTR_SPEED As Short = (2)
	Public Const TTSATTR_VOLUME As Short = (3)
	Public Const TTSATTR_PITCHRANGE As Integer = (TTSBASEATTR + 5)
	Public Const TTSATTR_PITCHRANGEDEFAULT As Integer = (TTSBASEATTR + 6)
	Public Const TTSATTR_PITCHRANGEMAX As Integer = (TTSBASEATTR + 7)
	Public Const TTSATTR_PITCHRANGEMIN As Integer = (TTSBASEATTR + 8)
	Public Const TTSATTR_PITCHRANGERELATIVE As Integer = (TTSBASEATTR + 9)
	Public Const TTSATTR_PITCHRANGERELATIVEMAX As Integer = (TTSBASEATTR + 10)
	Public Const TTSATTR_PITCHRANGERELATIVEMIN As Integer = (TTSBASEATTR + 11)
	Public Const TTSATTR_PITCHRELATIVE As Integer = (TTSBASEATTR + 12)
	Public Const TTSATTR_PITCHRELATIVEMAX As Integer = (TTSBASEATTR + 13)
	Public Const TTSATTR_PITCHRELATIVEMIN As Integer = (TTSBASEATTR + 14)
	Public Const TTSATTR_PITCHDEFAULT As Integer = (TTSBASEATTR + 15)
	Public Const TTSATTR_PITCHMAX As Integer = (TTSBASEATTR + 16)
	Public Const TTSATTR_PITCHMIN As Integer = (TTSBASEATTR + 17)
	Public Const TTSATTR_SPEEDRELATIVE As Integer = (TTSBASEATTR + 18)
	Public Const TTSATTR_SPEEDRELATIVEMAX As Integer = (TTSBASEATTR + 19)
	Public Const TTSATTR_SPEEDRELATIVEMIN As Integer = (TTSBASEATTR + 20)
	Public Const TTSATTR_SPEEDDEFAULT As Integer = (TTSBASEATTR + 21)
	Public Const TTSATTR_SPEEDMAX As Integer = (TTSBASEATTR + 22)
	Public Const TTSATTR_SPEEDMIN As Integer = (TTSBASEATTR + 23)
	Public Const TTSATTR_THREADPRIORITY As Integer = (TTSBASEATTR + 24)
	Public Const TTSATTR_SINKFLAGS As Integer = (TTSBASEATTR + 25)
	Public Const TTSATTR_VOLUMEDEFAULT As Integer = (TTSBASEATTR + 26)
	
	Public Const SRATTR_AUTOGAIN As Short = (1)
	Public Const SRATTR_ECHO As Short = (3)
	Public Const SRATTR_ENERGYFLOOR As Short = (4)
	Public Const SRATTR_MICROPHONE As Short = (5)
	Public Const SRATTR_REALTIME As Short = (6)
	Public Const SRATTR_SPEAKER As Short = (7)
	Public Const SRATTR_TIMEOUT_COMPLETE As Short = (8)
	Public Const SRATTR_TIMEOUT_INCOMPLETE As Integer = (SRBASEATTR + 8)
	Public Const SRATTR_THRESHOLD As Short = (2)
	Public Const SRATTR_ACCURACYSLIDER As Integer = (SRBASEATTR + 10)
	Public Const SRATTR_LEVEL As Integer = (SRBASEATTR + 11)
	Public Const SRATTR_LISTENINGSTATE As Integer = (SRBASEATTR + 12)
	Public Const SRATTR_RESULTSINFO As Integer = (SRBASEATTR + 13)
	Public Const SRATTR_RESULTSINFO_POSSIBLE As Integer = (SRBASEATTR + 14)
	Public Const SRATTR_SINKFLAGS As Integer = (SRBASEATTR + 15)
	Public Const SRATTR_THREADPRIORITY As Integer = (SRBASEATTR + 16)
	
	Public Const VDCTATTR_AWAKESTATE As Integer = (VDCTBASEATTR + 1)
	Public Const VDCTATTR_MODE As Integer = (VDCTBASEATTR + 2)
	Public Const VDCTATTR_MEMORY As Integer = (VDCTBASEATTR + 3)
	Public Const VDCTATTR_CORRECTIONRECT As Integer = (VDCTBASEATTR + 4)
	
	Public Const VCMDATTR_AWAKESTATE As Integer = (VCMDBASEATTR + 1)
	Public Const VCMDATTR_DEVICE As Integer = (VCMDBASEATTR + 2)
	Public Const VCMDATTR_ENABLED As Integer = (VCMDBASEATTR + 3)
	Public Const VCMDATTR_SRMODE As Integer = (VCMDBASEATTR + 4)
	
	Public Const AUDATTR_USELOWPRIORITY As Integer = (AUDBASEATTR + 1)
	Public Const AUDATTR_AUTORETRY As Integer = (AUDBASEATTR + 2)
	
	Public Const SRRI_AUDIO As Short = 1
	Public Const SRRI_AUDIO_UNCOMPRESSED As Short = 2
	Public Const SRRI_ALTERNATIVES As Short = 4
	Public Const SRRI_WORDGRAPH As Short = 8
	Public Const SRRI_PHONEMEGRAPH As Short = 16
	
	Public Const SRASF_ATTRIBUTES As Short = 1
	Public Const SRASF_INTERFERENCE As Short = 2
	Public Const SRASF_SOUND As Short = 4
	Public Const SRASF_UTTERANCEBEGIN As Short = 8
	Public Const SRASF_UTTERANCEEND As Short = 16
	Public Const SRASF_VUMETER As Short = 32
	Public Const SRASF_PHRASEHYPOTHESIS As Short = 64
	Public Const SRASF_TRAINING As Short = 128
	Public Const SRASF_ERRORWARNING As Short = 256
	
	Public Const ILP2_ACTIVE As Short = 1
	Public Const ILP2_USER As Short = 2
	Public Const ILP2_BACKUP As Short = 4
	Public Const ILP2_LTS As Short = 8
	
	Public Const STMWU_CNC As Short = 0
	Public Const STMWU_DICTATION As Short = 1
	Public Const STMWU_LOWERGAIN As Integer = &H10000
	Public Const STMWU_NOAUTOGAIN As Integer = &H20000
	
	Public Const STMWF_CANSKIP As Short = 1
	
	Public Const STMWI_UNKNOWN As Short = 0
	Public Const STMWI_CLOSETALK As Short = 1
	Public Const STMWI_EARPIECE As Short = 2
	Public Const STMWI_HANDSET As Short = 3
	Public Const STMWI_CLIPON As Short = 4
	Public Const STMWI_DESKTOP As Short = 5
	Public Const STMWI_HANDHELD As Short = 6
	Public Const STMWI_TOPMONITOR As Short = 7
	Public Const STMWI_INMONITOR As Short = 8
	Public Const STMWI_KEYBOARD As Short = 9
	Public Const STMWI_REMOTE As Short = 10
	
	Public Const STMWIS_UNKNOWN As Short = 0
	Public Const STMWIS_SPEAKERS As Short = 1
	Public Const STMWIS_HEADPHONES As Short = 2
	Public Const STMWIS_BOTH As Short = 3
	
	Public Const STLD_DISABLEREMOVE As Short = 1
	Public Const STLD_DISABLEADD As Short = 2
	Public Const STLD_FORCEEDIT As Short = 4
	Public Const STLD_DISABLEPRONADDREMOVE As Short = 8
	Public Const STLD_TEST As Short = 16
	Public Const STLD_DISABLERENAME As Short = 32
	Public Const STLD_CHANGEPRONADDS As Short = 64
	Public Const IANSRSN_NODATA As Short = 0
	Public Const IANSRSN_PRIORITY As Short = 1
	Public Const IANSRSN_INACTIVE As Short = 2
	Public Const IANSRSN_EOF As Short = 3
	
	Public Const IASISTATE_PASSTHROUGH As Short = 0
	Public Const IASISTATE_PASSNOTHING As Short = 1
	Public Const IASISTATE_PASSREADFROMWAVE As Short = 2
	Public Const IASISTATE_PASSWRITETOWAVE As Short = 3
	
	Public Const SRMI_NAMELEN As Short = SVFN_LEN
	
	Public Const SRSEQUENCE_DISCRETE As Short = (0)
	Public Const SRSEQUENCE_CONTINUOUS As Short = (1)
	Public Const SRSEQUENCE_WORDSPOT As Short = (2)
	Public Const SRSEQUENCE_CONTCFGDISCDICT As Short = (3)
	
	Public Const SRGRAM_CFG As Short = 1
	Public Const SRGRAM_DICTATION As Short = 2
	Public Const SRGRAM_LIMITEDDOMAIN As Short = 4
	
	Public Const SRFEATURE_INDEPSPEAKER As Short = 1
	Public Const SRFEATURE_INDEPMICROPHONE As Short = 2
	Public Const SRFEATURE_TRAINWORD As Short = 4
	Public Const SRFEATURE_TRAINPHONETIC As Short = 8
	Public Const SRFEATURE_WILDCARD As Short = 16
	Public Const SRFEATURE_ANYWORD As Short = 32
	Public Const SRFEATURE_PCOPTIMIZED As Short = 64
	Public Const SRFEATURE_PHONEOPTIMIZED As Short = 128
	Public Const SRFEATURE_GRAMLIST As Short = 256
	Public Const SRFEATURE_GRAMLINK As Short = 512
	Public Const SRFEATURE_MULTILINGUAL As Short = 1024
	Public Const SRFEATURE_GRAMRECURSIVE As Short = 2048
	Public Const SRFEATURE_IPAUNICODE As Short = 4096
	Public Const SRFEATURE_SINGLEINSTANCE As Short = 8192
	Public Const SRFEATURE_THREADSAFE As Short = 16384
	Public Const SRFEATURE_FIXEDAUDIO As Integer = 32768
	Public Const SRFEATURE_IPAWORD As Integer = 65536
	Public Const SRFEATURE_SAPI4 As Integer = 131072
	
	Public Const SRI_ILEXPRONOUNCE As Short = 1
	Public Const SRI_ISRATTRIBUTES As Short = 2
	Public Const SRI_ISRCENTRAL As Short = 4
	Public Const SRI_ISRDIALOGS As Short = 8
	Public Const SRI_ISRGRAMCOMMON As Short = 16
	Public Const SRI_ISRGRAMCFG As Short = 32
	Public Const SRI_ISRGRAMDICTATION As Short = 64
	Public Const SRI_ISRGRAMINSERTIONGUI As Short = 128
	Public Const SRI_ISRESBASIC As Short = 256
	Public Const SRI_ISRESMERGE As Short = 512
	Public Const SRI_ISRESAUDIO As Short = 1024
	Public Const SRI_ISRESCORRECTION As Short = 2048
	Public Const SRI_ISRESEVAL As Short = 4096
	Public Const SRI_ISRESGRAPH As Short = 8192
	Public Const SRI_ISRESMEMORY As Short = 16384
	Public Const SRI_ISRESMODIFYGUI As Integer = 32768
	Public Const SRI_ISRESSPEAKER As Integer = 65536
	Public Const SRI_ISRSPEAKER As Integer = 131072
	Public Const SRI_ISRESSCORES As Integer = 262144
	Public Const SRI_ISRESAUDIOEX As Integer = 524288
	Public Const SRI_ISRGRAMLEXPRON As Integer = 1048576
	Public Const SRI_ISRRESGRAPHEX As Integer = 2097152
	Public Const SRI_ILEXPRONOUNCE2 As Integer = 4194304
	Public Const SRI_IATTRIBUTES As Integer = 8388608
	
	Public Const SRGRAMQ_NONE As Short = 0
	Public Const SRGRAMQ_GENERALTRAIN As Short = 1
	Public Const SRGRAMQ_PHRASE As Short = 2
	Public Const SRGRAMQ_DIALOG As Short = 3
	
	Public Const ISRNOTEFIN_RECOGNIZED As Short = 1
	Public Const ISRNOTEFIN_THISGRAMMAR As Short = 2
	Public Const ISRNOTEFIN_FROMTHISGRAMMAR As Short = 4
	
	Public Const SRGNSTRAIN_GENERAL As Short = 1
	Public Const SRGNSTRAIN_GRAMMAR As Short = 2
	Public Const SRGNSTRAIN_MICROPHONE As Short = 4
	
	Public Const ISRNSAC_AUTOGAINENABLE As Short = 1
	Public Const ISRNSAC_THRESHOLD As Short = 2
	Public Const ISRNSAC_ECHO As Short = 3
	Public Const ISRNSAC_ENERGYFLOOR As Short = 4
	Public Const ISRNSAC_MICROPHONE As Short = 5
	Public Const ISRNSAC_REALTIME As Short = 6
	Public Const ISRNSAC_SPEAKER As Short = 7
	Public Const ISRNSAC_TIMEOUT As Short = 8
	Public Const ISRNSAC_STARTLISTENING As Short = 9
	Public Const ISRNSAC_STOPLISTENING As Short = 10
	
	Public Const SRMSGINT_NOISE As Short = (&H1s)
	Public Const SRMSGINT_NOSIGNAL As Short = (&H2s)
	Public Const SRMSGINT_TOOLOUD As Short = (&H3s)
	Public Const SRMSGINT_TOOQUIET As Short = (&H4s)
	Public Const SRMSGINT_AUDIODATA_STOPPED As Short = (&H5s)
	Public Const SRMSGINT_AUDIODATA_STARTED As Short = (&H6s)
	Public Const SRMSGINT_IAUDIO_STARTED As Short = (&H7s)
	Public Const SRMSGINT_IAUDIO_STOPPED As Short = (&H8s)
	
	Public Const SRHDRTYPE_CFG As Short = 0
	Public Const SRHDRTYPE_LIMITEDDOMAIN As Short = 1
	Public Const SRHDRTYPE_DICTATION As Short = 2
	
	Public Const SRHDRFLAG_UNICODE As Short = 1
	
	Public Const SRRESCUE_COMMA As Short = 1
	Public Const SRRESCUE_DECLARATIVEBEGIN As Short = 2
	Public Const SRRESCUE_DECLARATIVEEND As Short = 3
	Public Const SRRESCUE_IMPERATIVEBEGIN As Short = 4
	Public Const SRRESCUE_IMPERATIVEEND As Short = 5
	Public Const SRRESCUE_INTERROGATIVEBEGIN As Short = 6
	Public Const SRRESCUE_INTERROGATIVEEND As Short = 7
	Public Const SRRESCUE_NOISE As Short = 8
	Public Const SRRESCUE_PAUSE As Short = 9
	Public Const SRRESCUE_SENTENCEBEGIN As Short = 10
	Public Const SRRESCUE_SENTENCEEND As Short = 11
	Public Const SRRESCUE_UM As Short = 12
	Public Const SRRESCUE_WILDCARD As Short = 13
	Public Const SRRESCUE_WORD As Short = 14
	
	Public Const SRCFG_STARTOPERATION As Short = (1)
	Public Const SRCFG_ENDOPERATION As Short = (2)
	Public Const SRCFG_WORD As Short = (3)
	Public Const SRCFG_RULE As Short = (4)
	Public Const SRCFG_WILDCARD As Short = (5)
	Public Const SRCFG_LIST As Short = (6)
	
	Public Const SRCFGO_SEQUENCE As Short = (1)
	Public Const SRCFGO_ALTERNATIVE As Short = (2)
	Public Const SRCFGO_REPEAT As Short = (3)
	Public Const SRCFGO_OPTIONAL As Short = (4)
	
	Public Const SRCK_LANGUAGE As Short = 1
	Public Const SRCKCFG_WORDS As Short = 2
	Public Const SRCKCFG_RULES As Short = 3
	Public Const SRCKCFG_EXPORTRULES As Short = 4
	Public Const SRCKCFG_IMPORTRULES As Short = 5
	Public Const SRCKCFG_LISTS As Short = 6
	Public Const SRCKD_TOPIC As Short = 7
	Public Const SRCKD_COMMON As Short = 8
	Public Const SRCKD_GROUP As Short = 9
	Public Const SRCKD_SAMPLE As Short = 10
	Public Const SRCKLD_WORDS As Short = 11
	Public Const SRCKLD_GROUP As Short = 12
	Public Const SRCKLD_SAMPLE As Short = 13
	Public Const SRCKD_WORDCOUNT As Short = 14
	Public Const SRCKD_NGRAM As Short = 15
	
	Public Const SRTQEX_REQUIRED As Short = (&H0s)
	Public Const SRTQEX_RECOMMENDED As Short = (&H1s)
	
	Public Const SRAUDIOTIMESTAMP_DEFAULT As Short = -1
	
	Public Const SRCORCONFIDENCE_SOME As Short = 1
	Public Const SRCORCONFIDENCE_VERY As Short = 2
	
	Public Const SRGEX_ACOUSTICONLY As Short = 1
	Public Const SRGEX_LMONLY As Short = 2
	Public Const SRGEX_ACOUSTICANDLM As Short = 4
	
	Public Const SRRESMEMKIND_AUDIO As Short = 1
	Public Const SRRESMEMKIND_CORRECTION As Short = 2
	Public Const SRRESMEMKIND_EVAL As Short = 4
	Public Const SRRESMEMKIND_PHONEMEGRAPH As Short = 8
	Public Const SRRESMEMKIND_WORDGRAPH As Short = 16
	
	Public Const SRATTR_MINAUTOGAIN As Short = 0
	Public Const SRATTR_MAXAUTOGAIN As Short = 100
	Public Const SRATTR_MINENERGYFLOOR As Short = 0
	Public Const SRATTR_MAXENERGYFLOOR As Short = &HFFFFs
	Public Const SRATTR_MINREALTIME As Short = 0
	Public Const SRATTR_MAXREALTIME As Integer = &HFFFFFFFF
	Public Const SRATTR_MINTHRESHOLD As Short = 0
	Public Const SRATTR_MAXTHRESHOLD As Short = 100
	Public Const SRATTR_MINTOINCOMPLETE As Short = 0
	Public Const SRATTR_MAXTOINCOMPLETE As Integer = &HFFFFFFFF
	Public Const SRATTR_MINTOCOMPLETE As Short = 0
	Public Const SRATTR_MAXTOCOMPLETE As Integer = &HFFFFFFFF
	
	Public Const SRGRMFMT_CFG As Short = &H0s
	Public Const SRGRMFMT_LIMITEDDOMAIN As Short = &H1s
	
	Public Const SRGRMFMT_DICTATION As Short = &H2s
	
	Public Const SRGRMFMT_CFGNATIVE As Short = &H8000s
	
	Public Const SRGRMFMT_LIMITEDDOMAINNATIVE As Short = &H8001s
	
	Public Const SRGRMFMT_DICTATIONNATIVE As Short = &H8002s
	
	Public Const TTSI_NAMELEN As Short = SVFN_LEN
	Public Const TTSI_STYLELEN As Short = SVFN_LEN
	
	Public Const GENDER_NEUTRAL As Short = (0)
	Public Const GENDER_FEMALE As Short = (1)
	Public Const GENDER_MALE As Short = (2)
	
	Public Const TTSFEATURE_ANYWORD As Short = 1
	Public Const TTSFEATURE_VOLUME As Short = 2
	Public Const TTSFEATURE_SPEED As Short = 4
	Public Const TTSFEATURE_PITCH As Short = 8
	Public Const TTSFEATURE_TAGGED As Short = 16
	Public Const TTSFEATURE_IPAUNICODE As Short = 32
	Public Const TTSFEATURE_VISUAL As Short = 64
	Public Const TTSFEATURE_WORDPOSITION As Short = 128
	Public Const TTSFEATURE_PCOPTIMIZED As Short = 256
	Public Const TTSFEATURE_PHONEOPTIMIZED As Short = 512
	Public Const TTSFEATURE_FIXEDAUDIO As Short = 1024
	Public Const TTSFEATURE_SINGLEINSTANCE As Short = 2048
	Public Const TTSFEATURE_THREADSAFE As Short = 4096
	Public Const TTSFEATURE_IPATEXTDATA As Short = 8192
	Public Const TTSFEATURE_PREFERRED As Short = 16384
	Public Const TTSFEATURE_TRANSPLANTED As Integer = 32768
	Public Const TTSFEATURE_SAPI4 As Integer = 65536
	
	Public Const TTSI_ILEXPRONOUNCE As Short = 1
	Public Const TTSI_ITTSATTRIBUTES As Short = 2
	Public Const TTSI_ITTSCENTRAL As Short = 4
	Public Const TTSI_ITTSDIALOGS As Short = 8
	Public Const TTSI_ATTRIBUTES As Short = 16
	
	Public Const TTSDATAFLAG_TAGGED As Short = 1
	
	Public Const TTSBNS_ABORTED As Short = 1
	
	Public Const TTSNSAC_REALTIME As Short = 0
	Public Const TTSNSAC_PITCH As Short = 1
	Public Const TTSNSAC_SPEED As Short = 2
	Public Const TTSNSAC_VOLUME As Short = 3
	
	
	Public Const TTSNSHINT_QUESTION As Short = 1
	Public Const TTSNSHINT_STATEMENT As Short = 2
	Public Const TTSNSHINT_COMMAND As Short = 4
	Public Const TTSNSHINT_EXCLAMATION As Short = 8
	Public Const TTSNSHINT_EMPHASIS As Short = 16
	
	Public Const TTSAGE_BABY As Short = 1
	Public Const TTSAGE_TODDLER As Short = 3
	Public Const TTSAGE_CHILD As Short = 6
	Public Const TTSAGE_ADOLESCENT As Short = 14
	Public Const TTSAGE_ADULT As Short = 30
	Public Const TTSAGE_ELDERLY As Short = 70
	
	Public Const TTSATTR_MINPITCH As Short = 0
	Public Const TTSATTR_MAXPITCH As Short = &HFFFFs
	Public Const TTSATTR_MINREALTIME As Short = 0
	Public Const TTSATTR_MAXREALTIME As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINSPEED As Short = 0
	Public Const TTSATTR_MAXSPEED As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINVOLUME As Short = 0
	Public Const TTSATTR_MAXVOLUME As Integer = &HFFFFFFFF
	
	Public Const VCMD_APPLEN As Short = 32
	Public Const VCMD_STATELEN As Short = VCMD_APPLEN
	Public Const VCMD_MICLEN As Short = VCMD_APPLEN
	Public Const VCMD_SPEAKERLEN As Short = VCMD_APPLEN
	
	Public Const VCMDMC_CREATE_TEMP As Short = &H1s
	Public Const VCMDMC_CREATE_NEW As Short = &H2s
	Public Const VCMDMC_CREATE_ALWAYS As Short = &H4s
	Public Const VCMDMC_OPEN_ALWAYS As Short = &H8s
	Public Const VCMDMC_OPEN_EXISTING As Short = &H10s
	
	Public Const VCMDRF_NOMESSAGES As Short = &H0s
	Public Const VCMDRF_ALLBUTVUMETER As Short = &H1s
	Public Const VCMDRF_VUMETER As Short = &H2s
	Public Const VCMDRF_ALLMESSAGES As Integer = (VCMDRF_ALLBUTVUMETER + VCMDRF_VUMETER)
	
	Public Const VCMDEF_DATABASE As Short = &H0s
	Public Const VCMDEF_ACTIVE As Short = &H1s
	Public Const VCMDEF_SELECTED As Short = &H2s
	Public Const VCMDEF_PERMANENT As Short = &H4s
	Public Const VCMDEF_TEMPORARY As Short = &H8s
	
	Public Const VWGFLAG_ASLEEP As Short = &H1s
	
	Public Const VCMDACT_NORMAL As Short = (&H8000s)
	Public Const VCMDACT_LOW As Short = (&H4000s)
	Public Const VCMDACT_HIGH As Short = (&HC000s)
	
	Public Const VCMDCMD_VERIFY As Short = &H1s
	Public Const VCMDCMD_DISABLED_TEMP As Short = &H2s
	Public Const VCMDCMD_DISABLED_PERM As Short = &H4s
	Public Const VCMDCMD_CANTRENAME As Short = &H8s
	
	Public Const VCMD_BY_POSITION As Short = &H1s
	Public Const VCMD_BY_IDENTIFIER As Short = &H2s
	
	Public Const IVCNSAC_AUTOGAINENABLE As Short = &H1s
	Public Const IVCNSAC_ENABLED As Short = &H2s
	Public Const IVCNSAC_AWAKE As Short = &H4s
	Public Const IVCNSAC_DEVICE As Short = &H8s
	Public Const IVCNSAC_MICROPHONE As Short = &H10s
	Public Const IVCNSAC_SPEAKER As Short = &H20s
	Public Const IVCNSAC_SRMODE As Short = &H40s
	Public Const IVCNSAC_THRESHOLD As Short = &H80s
	Public Const IVCNSAC_ORIGINAPP As Integer = &H10000
	
	Public Const IVTNSAC_DEVICE As Short = &H1s
	Public Const IVTNSAC_ENABLED As Short = &H2s
	Public Const IVTNSAC_SPEED As Short = &H4s
	Public Const IVTNSAC_VOLUME As Short = &H8s
	Public Const IVTNSAC_TTSMODE As Short = &H10s
	
	Public Const VSRMODE_OFF As Short = &H2s
	Public Const VSRMODE_DISABLED As Short = &H1s
	Public Const VSRMODE_CMDPAUSED As Short = &H4s
	Public Const VSRMODE_CMDONLY As Short = &H10s
	Public Const VSRMODE_DCTONLY As Short = &H20s
	Public Const VSRMODE_CMDANDDCT As Short = &H40s
	
	Public Const VDCT_TOPICNAMELEN As Short = 32
	
	
	Public Const VDCT_TEXTADDED As Short = &H1s
	Public Const VDCT_TEXTREMOVED As Short = &H2s
	Public Const VDCT_TEXTREPLACED As Short = &H4s
	
	Public Const VDCT_TEXTCLEAN As Integer = &H10000
	Public Const VDCT_TEXTKEEPRESULTS As Integer = &H20000
	
	Public Const VDCTGUIF_VISIBLE As Short = &H1s
	Public Const VDCTGUIF_DONTMOVE As Short = &H2s
	Public Const VDCTGUIF_ADDWORD As Short = &H4s
	
	Public Const VDCTFX_CAPFIRST As Short = &H1s
	Public Const VDCTFX_LOWERFIRST As Short = &H2s
	Public Const VDCTFX_TOGGLEFIRST As Short = &H3s
	Public Const VDCTFX_CAPALL As Short = &H4s
	Public Const VDCTFX_LOWERALL As Short = &H5s
	Public Const VDCTFX_REMOVESPACES As Short = &H6s
	Public Const VDCTFX_KEEPONLYFIRSTLETTER As Short = &H7s
	Public Const VTXTST_STATEMENT As Short = &H1s
	Public Const VTXTST_QUESTION As Short = &H2s
	Public Const VTXTST_COMMAND As Short = &H4s
	Public Const VTXTST_WARNING As Short = &H8s
	Public Const VTXTST_READING As Short = &H10s
	Public Const VTXTST_NUMBERS As Short = &H20s
	Public Const VTXTST_SPREADSHEET As Short = &H40s
	
	Public Const VTXTSP_VERYHIGH As Short = &H80s
	Public Const VTXTSP_HIGH As Short = &H100s
	Public Const VTXTSP_NORMAL As Short = &H200s
	
	Public Const TTS_LANGUAGE As Short = 1
	Public Const TTS_VOICE As Short = 2
	Public Const TTS_GENDER As Short = 4
	Public Const TTS_VOLUME As Short = 8
	Public Const TTS_PITCH As Short = 16
	Public Const TTS_SPEED As Short = 32
	Public Const TTS_ABBREVIATION As Short = 64
	Public Const TTS_PUNCTUATION As Short = 128
	Public Const TTS_PAUSEWORD As Short = 256
	Public Const TTS_PAUSEPHRASE As Short = 512
	Public Const TTS_PAUSESENTENCE As Short = 1024
	Public Const TTS_SPELLING As Short = 2048
	Public Const TTS_QUALITY As Short = 4096
	Public Const TTS_FRICATION As Short = 8192
	Public Const TTS_ASPIRATION As Short = 16384
	Public Const TTS_INTONATION As Integer = 32768
	
	Public Const TTSATTR_MINPAUSEWORD As Short = &H0s
	Public Const TTSATTR_MAXPAUSEWORD As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINPAUSEPHRASE As Short = &H0s
	Public Const TTSATTR_MAXPAUSEPHRASE As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINPAUSESENTENCE As Short = &H0s
	Public Const TTSATTR_MAXPAUSESENTENCE As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINASPIRATION As Short = &H0s
	Public Const TTSATTR_MAXASPIRATION As Integer = &HFFFFFFFF
	Public Const TTSATTR_MINFRICATION As Short = &H0s
	Public Const TTSATTR_MAXFRICATION As Integer = &HFFFFFFFF
	Public Const TTSATTR_MININTONATION As Short = &H0s
	Public Const TTSATTR_MAXINTONATION As Integer = &HFFFFFFFF
	
	Public Const TTSNSAC_LANGUAGE As Short = 100
	Public Const TTSNSAC_VOICE As Short = 101
	Public Const TTSNSAC_GENDER As Short = 102
	Public Const TTSNSAC_ABBREVIATION As Short = 103
	Public Const TTSNSAC_PUNCTUATION As Short = 104
	Public Const TTSNSAC_PAUSEWORD As Short = 105
	Public Const TTSNSAC_PAUSEPHRASE As Short = 106
	Public Const TTSNSAC_PAUSESENTENCE As Short = 107
	Public Const TTSNSAC_SPELLING As Short = 108
	Public Const TTSNSAC_QUALITY As Short = 109
	Public Const TTSNSAC_FRICATION As Short = 110
	Public Const TTSNSAC_ASPIRATION As Short = 111
	Public Const TTSNSAC_INTONATION As Short = 112
	Public Const TTSI_ITTSEXTERNALSYNTHESIZER As Short = 16
	Public Const TTSERR_SYNTHESIZERBUSY As Integer = &H80004100
	Public Const TTSERR_ALREADYDISPLAYED As Integer = &H80004101
	Public Const TTSERR_INVALIDATTRIB As Integer = &H80004102
	Public Const TTSERR_SYNTHESIZERACCESSERROR As Integer = &H80004103
	Public Const TTSERR_DRIVERERROR As Integer = &H80004104
	Public Const TTSERR_UNRECOVERABLEERROR As Integer = &H80004105
	Public Const TTSERR_DRIVERACCESSERROR As Integer = &H80004106
	Public Const TTSERR_BUFFERTOOSMALL As Integer = &H80004107
	Public Const TTSERR_DRIVERNOTFOUND As Integer = &H80004108
	Public Const TTSERR_CANNOTREGISTER As Integer = &H80004109
	Public Const TTSERR_LANGUAGENOTSUPPORTED As Integer = &H80004110
End Module