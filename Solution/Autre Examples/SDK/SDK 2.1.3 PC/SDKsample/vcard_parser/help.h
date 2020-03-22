#ifndef _HELP_H_
#define _HELP_H_

#define VCARD_FILTER_INVALID				0xFF
#define VCARD_FILTER_VERSION				0x00
#define VCARD_FILTER_FN						0x01
#define VCARD_FILTER_N						0x02
#define VCARD_FILTER_PHOTO					0x03
#define VCARD_FILTER_BDAY					0x04
#define VCARD_FILTER_ADR					0x05
#define VCARD_FILTER_LABEL					0x06
#define VCARD_FILTER_TEL					0x07
#define VCARD_FILTER_EMAIL					0x08
#define VCARD_FILTER_MAILER					0x09
#define VCARD_FILTER_TZ						0x0A
#define VCARD_FILTER_GEO					0x0B
#define VCARD_FILTER_TITLE					0x0C
#define VCARD_FILTER_ROLE					0x0D
#define VCARD_FILTER_LOGO					0x0E
#define VCARD_FILTER_AGENT					0x0F
#define VCARD_FILTER_ORG					0x10
#define VCARD_FILTER_NOTE					0x11
#define VCARD_FILTER_REV					0x12
#define VCARD_FILTER_SOUND					0x13
#define VCARD_FILTER_URL					0x14
#define VCARD_FILTER_UID					0x15
#define VCARD_FILTER_KEY					0x16
#define VCARD_FILTER_NICKNAME				0x17
#define VCARD_FILTER_CATEGORIES				0x18
#define VCARD_FILTER_PROID					0x19
#define VCARD_FILTER_CLASS					0x1A
#define VCARD_FILTER_SORT_STRING			0x1B
#define VCARD_FILTER_X_IRMC_CALL_DATETIME	0x1C
#define VCARD_FILTER_PROPRIETARY_FILTER		0x27
struct FilterStru{
	char flag;
	char* prop_name;
};
static const struct FilterStru filter_table[] = {
		{ VCARD_FILTER_VERSION,					"VERSION"	},
		{ VCARD_FILTER_FN,						"FN"		},
		{ VCARD_FILTER_N,						"N"			},
		{ VCARD_FILTER_PHOTO,					"PHOTO"		},
		{ VCARD_FILTER_BDAY,					"BDAY"		},
		{ VCARD_FILTER_ADR,						"ADR"		},
		{ VCARD_FILTER_LABEL,					"LABEL"		},
		{ VCARD_FILTER_TEL,						"TEL"		},
		{ VCARD_FILTER_EMAIL,					"EMAIL"		},
		{ VCARD_FILTER_MAILER,					"MAILER"	},
		{ VCARD_FILTER_TZ,						"TZ"		},
		{ VCARD_FILTER_GEO,						"GEO"		},
		{ VCARD_FILTER_TITLE,					"TITLE"		},
		{ VCARD_FILTER_ROLE,					"ROLE"		},
		{ VCARD_FILTER_LOGO,					"LOGO"		},
		{ VCARD_FILTER_AGENT,					"AGENT"		},	
		{ VCARD_FILTER_ORG,						"ORG"		},
		{ VCARD_FILTER_NOTE,					"NOTE"		},
		{ VCARD_FILTER_REV,						"REV"		},
		{ VCARD_FILTER_SOUND,					"SOUND"		},	
		{ VCARD_FILTER_URL,						"URL"		},
		{ VCARD_FILTER_UID,						"UID"		},
		{ VCARD_FILTER_KEY,						"KEY"		},
		{ VCARD_FILTER_NICKNAME,				"NICKNAME"	},
		{ VCARD_FILTER_CATEGORIES,				"CATEGORIES"},	
		{ VCARD_FILTER_PROID,					"PROID"		},	
		{ VCARD_FILTER_CLASS,					"CLASS"		},	
		{ VCARD_FILTER_SORT_STRING,				"SORT-STRING"	},
		{ VCARD_FILTER_X_IRMC_CALL_DATETIME,	"X-IRMC-CALL-DATETIME"},
		{ VCARD_FILTER_PROPRIETARY_FILTER,		NULL	},
};
void writeVObjectWithFilters(FILE *fp, VObject *o, char* filters);
void writeVObjectWithFilters_(void *fp, VObject *o, char* filters);
char isFilterSigned(char* id, char* filters);
char IsPropertySigned(const char* filter, char prop_index);

VObject* CopyGrop(VObject* o, VObject* g);
VObject* CopyAttrValue(VObject* o, VObject* a);
VObject* CopyValue(VObject* o, VObject* a, int size);

#endif