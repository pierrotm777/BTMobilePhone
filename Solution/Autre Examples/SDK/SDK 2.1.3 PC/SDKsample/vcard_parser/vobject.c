/***************************************************************************
(C) Copyright 1996 Apple Computer, Inc., AT&T Corp., International             
Business Machines Corporation and Siemens Rolm Communications Inc.             

For purposes of this license notice, the term Licensors shall mean,            
collectively, Apple Computer, Inc., AT&T Corp., International                  
Business Machines Corporation and Siemens Rolm Communications Inc.             
The term Licensor shall mean any of the Licensors.                             

Subject to acceptance of the following conditions, permission is hereby        
granted by Licensors without the need for written agreement and without        
license or royalty fees, to use, copy, modify and distribute this              
software for any purpose.                                                      

The above copyright notice and the following four paragraphs must be           
reproduced in all copies of this software and any software including           
this software.                                                                 

THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS AND NO LICENSOR SHALL HAVE       
ANY OBLIGATION TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS OR       
MODIFICATIONS.                                                                 

IN NO EVENT SHALL ANY LICENSOR BE LIABLE TO ANY PARTY FOR DIRECT,              
INDIRECT, SPECIAL OR CONSEQUENTIAL DAMAGES OR LOST PROFITS ARISING OUT         
OF THE USE OF THIS SOFTWARE EVEN IF ADVISED OF THE POSSIBILITY OF SUCH         
DAMAGE.                                                                        

EACH LICENSOR SPECIFICALLY DISCLAIMS ANY WARRANTIES, EXPRESS OR IMPLIED,       
INCLUDING BUT NOT LIMITED TO ANY WARRANTY OF NONINFRINGEMENT OR THE            
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR             
PURPOSE.                                                                       

The software is provided with RESTRICTED RIGHTS.  Use, duplication, or         
disclosure by the government are subject to restrictions set forth in          
DFARS 252.227-7013 or 48 CFR 52.227-19, as applicable.                         

***************************************************************************/

/*
* src: vobject.c
* doc: vobject and APIs to construct vobject, APIs pretty print 
* vobject, and convert a vobject into its textual representation.
*/

/*
* object.c implements an API that insulates the caller from
* the parser and changes in the vCard/vCalendar BNF  
*/

/* 
* vcaltmp.h and vcaltmp.c implement vCalendar "macro" functions
* which you may find useful.
*/

#ifndef	 MWERKS
#include <malloc.h>
#endif

#include "vobject.h"
#include "vcc.h"
#include <string.h>
#include <stdio.h>
#include <fcntl.h>
#include <io.h>
#include "help.h"
//#include "global.h"//temp test

#define NAME_OF(o)				o->id
#define VALUE_TYPE(o)			o->valType
#define STRINGZ_VALUE_OF(o)		o->val.strs
#define USTRINGZ_VALUE_OF(o)	o->val.ustrs
#define INTEGER_VALUE_OF(o)		o->val.i
#define LONG_VALUE_OF(o)		o->val.l
#define ANY_VALUE_OF(o)			o->val.any
#define VOBJECT_VALUE_OF(o)		o->val.vobj


/* how to use this item?*/
typedef struct StrItem StrItem;

struct StrItem {
	StrItem *next;
	const char *s;
	unsigned int refCnt;
};

const char** fieldedProp;

void writeVObjectWithFilters(FILE *fp, VObject *o, char* filters);
void writeVObjectWithFilters_(void *fp, VObject *o, char* filters);
char isFilterSigned(char* id, char* filters);
char IsPropertySigned(const char* filter, char prop_index);
/*----------------------------------------------------------------------
The following functions involve with memory allocation:
newVObject
deleteVObject
dupStr
deleteStr
newStrItem
deleteStrItem
----------------------------------------------------------------------*/

DLLEXPORT(VObject*) newVObject_(const char *id)
{
	VObject *p = (VObject*)malloc(sizeof(VObject));
	p->next = 0;
	p->id = id;
	p->prop = 0;
	VALUE_TYPE(p) = 0;
	ANY_VALUE_OF(p) = 0;
	return p;
}
/*-------------------------------------------------------------
the following APIs are mainly used to construct a VObject tree:
--------------------------------------------------------------*/
/*
VObject* newVObject(const char *id);
-- used extensively internally by VObject APIs but when
used externally, its use is mainly limited to the
construction of top level object (e.g. an object
with VCCardProp or VCCalendarProp id).
*/
DLLEXPORT(VObject*) newVObject(const char *id)
{
	return newVObject_(lookupStr(id));
}
/* 
void deleteVObject(VObject *p);
-- to deallocate single VObject, for most user, use
cleanVObject(VObject *o) instead for freeing all
resources associated with the VObject.
*/
DLLEXPORT(void) deleteVObject(VObject *p)
{
	unUseStr(p->id);
	free(p);
}
/*
char* dupStr(const char *s, unsigned int size);
-- duplicate a string s. If size is 0, the string is
assume to be a null-terminated. 

add a \0 at the end of the new one
return a new one
*/
DLLEXPORT(char*) dupStr(const char *s, unsigned int size)
{
	char *t;
	if  (size == 0) {
		size = strlen(s);
	}
	t = (char*)malloc(size+1);
	if (t) {
		memcpy(t,s,size);
		t[size] = 0;
		/* \0 ? */
		return t;
	}
	else {
		return (char*)0;
	}
}
/*
void deleteStr(const char *p);
-- used to deallocate a string allocated by dupStr();
*/
DLLEXPORT(void) deleteStr(const char *p)
{
	if (p) free((void*)p);
}


static StrItem* newStrItem(const char *s, StrItem *next)
{
	StrItem *p = (StrItem*)malloc(sizeof(StrItem));
	p->next = next;
	p->s = s;
	p->refCnt = 1;
	return p;
}

static void deleteStrItem(StrItem *p)
{
	free((void*)p);
}


/*----------------------------------------------------------------------
The following function provide accesses to VObject's value.
----------------------------------------------------------------------*/

/*
Use the API vObjectName() to access a VObject's name.

const char* vObjectName(VObject *o);
-- retrieve the VObject's Name (i.e. id).
*/

DLLEXPORT(const char*) vObjectName(VObject *o)
{
	return NAME_OF(o);
}
/*
void setVObjectName(VObject *o, const char* id);
-- set the id of VObject o. This function is not
normally used by the user. The setting of id
is normally done as part of other APIs (e.g.
addProp()).
*/
DLLEXPORT(void) setVObjectName(VObject *o, const char* id)
{
	NAME_OF(o) = id;
}
/*------------------
Use the APIs vObject???Value() to access a VObject's value.
where ??? is the expected type.
Use the APIs setvObject???Value() to set or modify a VObject's value.
where ??? is the expected type.
*-------------------/
/*
const char* vObjectStringZValue(VObject *o);
-- retrieve the VObject's value interpreted as
null-terminated string.
*/
DLLEXPORT(const char*) vObjectStringZValue(VObject *o)
{
	return STRINGZ_VALUE_OF(o);
}
/*
void setVObjectStringZValue(VObject *o, const char *s);
-- set a string value of a VObject.
*/
DLLEXPORT(void) setVObjectStringZValue(VObject *o, const char *s)
{
	/* 
	use dupStr() function to return a new string which value equal
	to string s except that a '0' be added  at the end of the new one
	*/
	STRINGZ_VALUE_OF(o) = dupStr(s,0);
	VALUE_TYPE(o) = VCVT_STRINGZ;
}

DLLEXPORT(void) setVObjectStringZValue_(VObject *o, const char *s)
{
	STRINGZ_VALUE_OF(o) = s;
	VALUE_TYPE(o) = VCVT_STRINGZ;
}
/*
const wchar_t* vObjectUStringZValue(VObject *o);
-- retrieve the VObject's value interpreted as
null-terminated unicode string.
*/
DLLEXPORT(const wchar_t*) vObjectUStringZValue(VObject *o)
{
	return USTRINGZ_VALUE_OF(o);
}
/*
void setVObjectUStringZValue(VObject *o, const wchar_t *s);
-- set a Unicode string value of a VObject.
*/
DLLEXPORT(void) setVObjectUStringZValue(VObject *o, const wchar_t *s)
{
	/* uStrLen(s)+1) ??? \0 ? */
	USTRINGZ_VALUE_OF(o) = (wchar_t*) dupStr((char*)s,(uStrLen(s)+1)*2);
	VALUE_TYPE(o) = VCVT_USTRINGZ;
}

DLLEXPORT(void) setVObjectUStringZValue_(VObject *o, const wchar_t *s)
{
	USTRINGZ_VALUE_OF(o) = s;
	VALUE_TYPE(o) = VCVT_USTRINGZ;
}
/*
unsigned int vObjectIntegerValue(VObject *o);
-- retrieve the VObject's value interpreted as
integer.
*/
DLLEXPORT(unsigned int) vObjectIntegerValue(VObject *o)
{
	return INTEGER_VALUE_OF(o);
}
/*
void setVObjectIntegerValue(VObject *o, unsigned int i);
-- set an integer value of a VObject.
*/
DLLEXPORT(void) setVObjectIntegerValue(VObject *o, unsigned int i)
{
	INTEGER_VALUE_OF(o) = i;
	VALUE_TYPE(o) = VCVT_UINT;
}
/*
unsigned long vObjectLongValue(VObject *o);
-- retrieve the VObject's value interpreted as
long integer.	
*/
DLLEXPORT(unsigned long) vObjectLongValue(VObject *o)
{
	return LONG_VALUE_OF(o);
}
/*
void setVObjectLongValue(VObject *o, unsigned long l);
-- set an long integer value of a VObject.
*/
DLLEXPORT(void) setVObjectLongValue(VObject *o, unsigned long l)
{
	LONG_VALUE_OF(o) = l;
	VALUE_TYPE(o) = VCVT_ULONG;
}
/*
void* vObjectAnyValue(VObject *o);
-- retrieve the VObject's value interpreted as
any value.
*/
DLLEXPORT(void*) vObjectAnyValue(VObject *o)
{
	return ANY_VALUE_OF(o);
}
/*
void setVObjectAnyValue(VObject *o, void *t);
-- set any value of a VObject. The value type is
unspecified.	
*/
DLLEXPORT(void) setVObjectAnyValue(VObject *o, void *t)
{
	ANY_VALUE_OF(o) = t;
	VALUE_TYPE(o) = VCVT_RAW;
}
/*
VObject* vObjectVObjectValue(VObject *o);
-- retrieve the VObject's value interpreted as
a VObject.
*/
DLLEXPORT(VObject*) vObjectVObjectValue(VObject *o)
{
	return VOBJECT_VALUE_OF(o);
}
/*
void setVObjectVObjectValue(VObject *o, VObject *p);
-- set a VObject as the value of another VObject.

*/
DLLEXPORT(void) setVObjectVObjectValue(VObject *o, VObject *p)
{
	VOBJECT_VALUE_OF(o) = p;
	VALUE_TYPE(o) = VCVT_VOBJECT;
}
/*
Use the API vObjectValueType() to determine if a VObject has
a value. For VCard/VCalendar application, you
should not need this function as practically
all values are either of type VCVT_USTRINGZ or
VCVT_RAW (i.e set by setVObjectUStringZValue and
setVObjectAnyValue APIs respectively), and the
value returned by calls to vObjectUStringZValue
and vObjectAnyValue are 0 if a VObject has no
value. (There is a minor exception where VObject with
VCDataSizeProp has value that is set by
setVObjectLongValue).
*/
DLLEXPORT(int) vObjectValueType(VObject *o)
{
	return VALUE_TYPE(o);
}


/*----------------------------------------------------------------------
The following functions can be used to build VObject.
----------------------------------------------------------------------*/
/*
VObject* addVObjectProp(VObject *o, VObject *p);
-- add a VObject p as a property of VObject o.
(not normally used externally for building a
VObject).
*/
DLLEXPORT(VObject*) addVObjectProp(VObject *o, VObject *p)
{   /*
	why let it be a circular link ???
	*/
	/* circular link list pointed to tail */
	/*
	o {next,id,prop,val}
	V
	pn {next,id,prop,val}
	V
	...
	p1 {next,id,prop,val}
	V
	pn
	-->
	o {next,id,prop,val}
	V
	pn {next,id,prop,val}
	V
	p {next,id,prop,val}
	...
	p1 {next,id,prop,val}
	V
	pn
	*/
	/* I think it should be next,not id */

	VObject *tail = o->prop;
	if (tail) {
		p->next = tail->next;
		o->prop = tail->next = p;
		/*
		o->prop = p; ???
		*/
	}
	else {
		o->prop = p->next = p;
	}
	return p;
}
/*
VObject* addProp(VObject *o, const char *id);
-- add a property whose name is id to VObject o.
*/  
DLLEXPORT(VObject*) addProp(VObject *o, const char *id)
{
	return addVObjectProp(o,newVObject(id));
}

DLLEXPORT(VObject*) addProp_(VObject *o, const char *id)
{
	return addVObjectProp(o,newVObject_(id));
}
/*
VObject can be chained together to form a list. e.g. of such
use is in the parser where the return value of the parser is
a link list of VObject. A link list of VObject can be
built by:
void addList(VObject **o, VObject *p);
and iterated by
VObject* nextVObjectInList(VObject *o);
-- nextVObjectInList return 0 if the list
is exhausted.
*/
DLLEXPORT(void) addList(VObject **o, VObject *p)
{
	p->next = 0;
	if (*o == 0) {
		*o = p;
	}
	else {
		VObject *t = *o;
		while (t->next) {
			t = t->next;
		}
		t->next = p;
	}
}

DLLEXPORT(VObject*) nextVObjectInList(VObject *o)
{
	return o->next;
}

DLLEXPORT(VObject*) setValueWithSize_(VObject *prop, void *val, unsigned int size)
{
	VObject *sizeProp;
	setVObjectAnyValue(prop, val);
	sizeProp = addProp(prop,VCDataSizeProp);
	setVObjectLongValue(sizeProp, size);
	return prop;
}
/*
VObject* setValueWithSize(VObject *prop, void *val, unsigned int size);
-- set a raw data (stream of bytes) value of a VObject
whose size is size. The internal VObject representation
is
this object = val
VCDataSizeProp=size
i.e. the value val will be attached to the VObject prop
and a property of VCDataSize whose value is size
is also added to the object.
*/
DLLEXPORT(VObject*) setValueWithSize(VObject *prop, void *val, unsigned int size)
{
	void *p = dupStr((const char *)val,size);
	return setValueWithSize_(prop,p,p?size:0);
}
/*
typedef struct VObjectIterator {
VObject* start;
VObject* next;
} VObjectIterator;
*/
/*
================================================
The following is a skeletal form of iterating through
all properties of a vobject, o:

// assume the object of interest, o, is of type VObject
VObjectIterator i;
initPropIterator(&i,o);
while (moreIteration(&i)) {
VObject *each = nextVObject(&i);
// ... do something with "each" property 
}

Use the API vObjectName() to access a VObject's name.
Use the API vObjectValueType() to determine if a VObject has
a value. For VCard/VCalendar application, you
should not need this function as practically
all values are either of type VCVT_USTRINGZ or
VCVT_RAW (i.e set by setVObjectUStringZValue and
setVObjectAnyValue APIs respectively), and the
value returned by calls to vObjectUStringZValue
and vObjectAnyValue are 0 if a VObject has no
value. (There is a minor exception where VObject with
VCDataSizeProp has value that is set by
setVObjectLongValue).
Use the APIs vObject???Value() to access a VObject's value.
where ??? is the expected type.
Use the APIs setvObject???Value() to set or modify a VObject's value.
where ??? is the expected type.
Use the API isAPropertyOf() to query if a name match the name of
a property of a VObject. Since isAPropertyOf() return
the matching property, we can use that to retrieve
a property and subsequently the value of the property.

*/
DLLEXPORT(void) initPropIterator(VObjectIterator *i, VObject *o, char *id)
{
	if (id != NULL && strlen(id)) {
		i->id = malloc(strlen(id) + 1);
		strcpy(i->id, id);
	} else {
		i->id = 0;
	}
	i->start = o->prop; 
	i->next = 0;
}

DLLEXPORT(void) initVObjectIterator(VObjectIterator *i, VObject *o)
{
	i->start = o->next; 
	i->next = 0;
}

DLLEXPORT(int) moreIteration(VObjectIterator *i)
{ 
	return (i->start && (i->next==0 || i->next!=i->start));
}

DLLEXPORT(VObject*) nextVObject(VObjectIterator *i)
{
	if (i->start && i->next != i->start) {
		if (i->next == 0) {
			i->next = i->start->next;
			return i->next;
		}
		else {
			i->next = i->next->next;
			return i->next;
		}
	}
	else return (VObject*)0;
}
/*
Use the API isAPropertyOf() to query if a name match the name of
a property of a VObject. Since isAPropertyOf() return
the matching property, we can use that to retrieve
a property and subsequently the value of the property.

VObject* isAPropertyOf(VObject *o, const char *id);
-- query if a property by the name id is in o and
return the VObject that represent that property.
*/
DLLEXPORT(VObject*) isAPropertyOf(VObject *o, const char *id)
{
	VObjectIterator i;
	initPropIterator(&i,o,NULL);
	while (moreIteration(&i)) {
		VObject *each = nextVObject(&i);
		if (!stricmp(id,each->id))
			return each;
	}
	return (VObject*)0;
}
/*
VObject* addGroup(VObject *o, const char *g);
-- add a group g to VObject o.
e.g. if g is a.b.c, you will have
o
c
VCGroupingProp=b
VCGroupingProp=a
and the object c is returned.
*/

DLLEXPORT(VObject*) addGroup(VObject *o, const char *g)
{
	/*
	a.b.c
	-->
	prop(c)
	prop(VCGrouping=b)
	prop(VCGrouping=a)
	*/
	char *dot = strrchr(g,'.');
	if (dot) {
		VObject *p, *t;
		char *gs, *n = dot+1;
		gs = dupStr(g,0);	/* so we can write to it. */
		/* used to be
		* t = p = addProp_(o,lookupProp_(n));
		*/
		t = p = addProp_(o,lookupProp(n));
		dot = strrchr(gs,'.');
		*dot = 0;/* replace with \0,so... */
		do {
			dot = strrchr(gs,'.');
			if (dot) {
				n = dot+1;
				*dot=0;/* replace with \0,so... */
			}
			else
				n = gs;
			/* property(VCGroupingProp=n);
			*	and the value may have VCGrouping property
			*/
			t = addProp(t,VCGroupingProp);
			setVObjectStringZValue(t,lookupProp_(n));
		} while (n != gs);
		deleteStr(gs);	
		return p;
	}
	else
		return addProp_(o,lookupProp(g));
}
/*
VObject* addPropValue(VObject *o, const char *id, const char *v);
-- add a property whose name is id and whose value
is a null-terminated string to VObject o.

addPropValue(root,VCSomeProp,someASCIIStringZValue);
Note that someASCIISTringZValue is automatically converted to
Unicode by addPropValue API, where as, the former code
sequence do an explicit call to fakeUnicode.
*/
DLLEXPORT(VObject*) addPropValue(VObject *o, const char *p, const char *v)
{
	VObject *prop;
	prop = addProp(o,p);
	setVObjectUStringZValue_(prop, fakeUnicode(v,0));
	return prop;
}

DLLEXPORT(VObject*) addPropSizedValue_(VObject *o, const char *p, const char *v,
									   unsigned int size)
{
	VObject *prop;
	prop = addProp(o,p);
	setValueWithSize_(prop, (void*)v, size);
	return prop;
}
/*
VObject* addPropSizedValue(VObject *o, const char *id,
const char *v, unsigned int size);
-- add a property whose name is id and whose value
is a stream of bytes of size size, to VObject o.
*/
DLLEXPORT(VObject*) addPropSizedValue(VObject *o, const char *p, const char *v,
									  unsigned int size)
{
	return addPropSizedValue_(o,p,dupStr(v,size),size);
}



/*----------------------------------------------------------------------
The following pretty print a VObject
----------------------------------------------------------------------*/

static void printVObject_(FILE *fp, VObject *o, int level);
/*
PrintNameValue()
v
PrintValue()
v
PrintVobject_()  < printVObject() <= printVObjectToFile <= printVObjectsToFile
v
PrintNameValue()

a big recursive cycle
only printVObjectToFile() printVObjectsToFile() can be seen by user
*/
static void indent(FILE *fp, int level)
{
	int i;
	for (i=0;i<level*4;i++) {
		fputc(' ', fp);
	}
}

static void printValue(FILE *fp, VObject *o, int level)
{
	switch (VALUE_TYPE(o)) {
case VCVT_USTRINGZ: {
	char c;
	char *t,*s;
	s = t = fakeCString(USTRINGZ_VALUE_OF(o));
	fputc('"',fp);
	while (c=*t,c) {
		fputc(c,fp);
		if (c == '\n') indent(fp,level+2);
		t++;
	}
	fputc('"',fp);
	deleteStr(s);
	break;
					}
case VCVT_STRINGZ: {
	char c;
	const char *s = STRINGZ_VALUE_OF(o);
	fputc('"',fp);
	while (c=*s,c) {
		fputc(c,fp);
		if (c == '\n') indent(fp,level+2);
		s++;
	}
	fputc('"',fp);
	break;
				   }
case VCVT_UINT:
	fprintf(fp,"%d", INTEGER_VALUE_OF(o)); break;
case VCVT_ULONG:
	fprintf(fp,"%ld", LONG_VALUE_OF(o)); break;
case VCVT_RAW:
	fprintf(fp,"[raw data]"); break;
case VCVT_VOBJECT:
	fprintf(fp,"[vobject]\n");
	printVObject_(fp,VOBJECT_VALUE_OF(o),level+1);
	break;
case 0:
	fprintf(fp,"[none]"); break;
default:
	fprintf(fp,"[unknown]"); break;
	}
}

static void printNameValue(FILE *fp,VObject *o, int level)
{
	indent(fp,level);
	if (NAME_OF(o)) {
		fprintf(fp,"%s", NAME_OF(o));
	}
	if (VALUE_TYPE(o)) {
		fputc('=',fp);
		printValue(fp,o, level);
	}
	fprintf(fp,"\n");
}

static void printVObject_(FILE *fp, VObject *o, int level)
{
	VObjectIterator t;
	if (o == 0) {
		fprintf(fp,"[NULL]\n");
		return;
	}
	printNameValue(fp,o,level);
	initPropIterator(&t,o,NULL);
	while (moreIteration(&t)) {
		VObject *eachProp = nextVObject(&t);
		printVObject_(fp,eachProp,level+1);
	}
}
/*
????????
void printVObject(VObject *o);
-- pretty print VObject o to stdout (for debugging use).
*/
void printVObject(FILE *fp,VObject *o)
{
	printVObject_(fp,o,0);
}

DLLEXPORT(void) printVObjectToFile(char *fname,VObject *o)
{
	FILE *fp = fopen(fname,"w");
	if (fp) {
		printVObject(fp,o);
		fclose(fp);
	}
}

DLLEXPORT(void) printVObjectsToFile(char *fname,VObject *list)
{
	FILE *fp = fopen(fname,"w");
	if (fp) {
		while (list) {
			printVObject(fp,list);
			list = nextVObjectInList(list);
		}
		fclose(fp);
	}
}
/*
void cleanVObject(VObject *o);
-- release all resources used by VObject o.
*/
DLLEXPORT(void) cleanVObject(VObject *o)
{
	if (o == 0) return;
	if (o->prop) {
		/* destroy time: cannot use the iterator here.
		Have to break the cycle in the circular link
		list and turns it into regular NULL-terminated
		list -- since at some point of destruction,
		the reference entry for the iterator to work
		will not longer be valid.
		*/
		VObject *p;
		p = o->prop->next;
		o->prop->next = 0;
		do {
			VObject *t = p->next;
			/*RECURSIVE FUNCTION*/
			cleanVObject(p);
			p = t;
		} while (p);
	}
	switch (VALUE_TYPE(o)) {
case VCVT_USTRINGZ:
case VCVT_STRINGZ:
case VCVT_RAW:
	// assume they are all allocated by malloc.
	free((char*)STRINGZ_VALUE_OF(o));
	break;
case VCVT_VOBJECT:
	/*RECURSIVE FUNCTION*/
	cleanVObject(VOBJECT_VALUE_OF(o));
	break;
	}
	deleteVObject(o);
}

DLLEXPORT(void) cleanVObjects(VObject *list)
{
	while (list) {
		VObject *t = list;
		list = nextVObjectInList(list);
		cleanVObject(t);
	}
}

/*----------------------------------------------------------------------
The following is a String Table Facilities.
----------------------------------------------------------------------*/

#define STRTBLSIZE 255

static StrItem *strTbl[STRTBLSIZE];

static unsigned int hashStr(const char *s)
{
	unsigned int h = 0;
	int i;
	for (i=0;s[i];i++) {
		h += s[i]*i;
	}

	if (strTbl[h % STRTBLSIZE] != NULL) {
		for (i = 0; i < STRTBLSIZE; i++) {
			if (strTbl[i] == NULL) {
				return i;
			}
		}
	}

	return h % STRTBLSIZE;
}
/* add to the strTbl[h] */
DLLEXPORT(const char*) lookupStr(const char *s)
{
	StrItem *t;
	unsigned int h = hashStr(s);
	if ((t = strTbl[h]) != 0) {
		do {
			if (stricmp(t->s,s) == 0) {
				t->refCnt++;
				return t->s;
			}
			t = t->next;
		} while (t);
	}
	/* const ?!
	OK: pointer not declared const
	*/
	s = dupStr(s,0);
	strTbl[h] = newStrItem(s,strTbl[h]);
	return s;
}
/*
static StrItem* newStrItem(const char *s, StrItem *next)
{
StrItem *p = (StrItem*)malloc(sizeof(StrItem));
p->next = next;
p->s = s;
p->refCnt = 1;
return p;
}
*/
DLLEXPORT(void) unUseStr(const char *s)
{
	StrItem *t, *p;
	unsigned int h = hashStr(s);
	if ((t = strTbl[h]) != 0) {
		p = t;
		do {
			if (stricmp(t->s,s) == 0) {
				t->refCnt--;
				if (t->refCnt == 0) {
					if (p == strTbl[h]) {
						strTbl[h] = t->next;
					}
					else {
						p->next = t->next;
					}
					deleteStr(t->s);
					deleteStrItem(t);
					return;
				}
			}
			p = t;
			t = t->next;
		} while (t);
	}
}
/*
void cleanStrTbl();
-- this function has to be called when all
VObject has been destroyed.
*/
DLLEXPORT(void) cleanStrTbl()
{
	int i;
	for (i=0; i<STRTBLSIZE;i++) {
		StrItem *t = strTbl[i];
		while (t) {
			StrItem *p;
			if (t->s) {
				deleteStr(t->s);
				t->s = NULL;
			}
			p = t;
			t = t->next;
			deleteStrItem(p);
		}//while (t);
		/* this is a blah,but no negtive effect */
		strTbl[i] = 0;
	}
}


struct PreDefProp {
	const char *name;
	const char *alias;
	const char** fields;
	unsigned int flags;
};

/* flags in PreDefProp */
#define PD_BEGIN	0x1
#define PD_INTERNAL	0x2

/* can not be modified */
static const char *adrFields[] = {
	VCPostalBoxProp,
	VCExtAddressProp,
	VCStreetAddressProp,
	VCCityProp,
	VCRegionProp,
	VCPostalCodeProp,
	VCCountryNameProp,
	0
};

static const char *nameFields[] = {
	VCFamilyNameProp,
	VCGivenNameProp,
	VCAdditionalNamesProp,
	VCNamePrefixesProp,
	VCNameSuffixesProp,
	NULL
};

static const char *orgFields[] = {
	VCOrgNameProp,
	VCOrgUnitProp,
	VCOrgUnit2Prop,
	VCOrgUnit3Prop,
	VCOrgUnit4Prop,
	NULL
};

static const char *AAlarmFields[] = {
	VCRunTimeProp,
	VCSnoozeTimeProp,
	VCRepeatCountProp,
	VCAudioContentProp,
	0
};

/* ExDate -- has unamed fields */
/* RDate -- has unamed fields */

static const char *DAlarmFields[] = {
	VCRunTimeProp,
	VCSnoozeTimeProp,
	VCRepeatCountProp,
	VCDisplayStringProp,
	0
};

static const char *MAlarmFields[] = {
	VCRunTimeProp,
	VCSnoozeTimeProp,
	VCRepeatCountProp,
	VCEmailAddressProp,
	VCNoteProp,
	0
};

static const char *PAlarmFields[] = {
	VCRunTimeProp,
	VCSnoozeTimeProp,
	VCRepeatCountProp,
	VCProcedureNameProp,
	0
};

static struct PreDefProp propNames[] = {
	{ VC7bitProp, 0, 0, 0 },
	{ VC8bitProp, 0, 0, 0 },
	{ VCAAlarmProp, 0, AAlarmFields, 0 },
	{ VCAdditionalNamesProp, 0, 0, 0 },
	{ VCAdrProp, 0, adrFields, 0 },
	{ VCAgentProp, 0, 0, 0 },
	{ VCAIFFProp, 0, 0, 0 },
	{ VCAOLProp, 0, 0, 0 },
	{ VCAppleLinkProp, 0, 0, 0 },
	{ VCAttachProp, 0, 0, 0 },
	{ VCAttendeeProp, 0, 0, 0 },
	{ VCATTMailProp, 0, 0, 0 },
	{ VCAudioContentProp, 0, 0, 0 },
	{ VCAVIProp, 0, 0, 0 },
	{ VCBase64Prop, 0, 0, 0 },
	{ VCBBSProp, 0, 0, 0 },
	{ VCBirthDateProp, 0, 0, 0 },
	{ VCBMPProp, 0, 0, 0 },
	//?
	{ VCBodyProp, 0, 0, 0 },
	{ VCBusinessRoleProp, 0, 0, 0 },
	//
	{ VCCalProp, 0, 0, PD_BEGIN },
	{ VCCaptionProp, 0, 0, 0 },
	//
	{ VCCardProp, 0, 0, PD_BEGIN },
	{ VCCarProp, 0, 0, 0 },
	//
	{ VCCategoriesProp, 0, 0, 0 },
	{ VCCellularProp, 0, 0, 0 },
	{ VCCGMProp, 0, 0, 0 },
	{ VCCharSetProp, 0, 0, 0 },
	{ VCCIDProp, VCContentIDProp, 0, 0 },
	{ VCCISProp, 0, 0, 0 },
	{ VCCityProp, 0, 0, 0 },
	//
	{ VCClassProp, 0, 0, 0 },
	{ VCCommentProp, 0, 0, 0 },
	{ VCCompletedProp, 0, 0, 0 },
	{ VCContentIDProp, 0, 0, 0 },
	{ VCCountryNameProp, 0, 0, 0 },
	{ VCDAlarmProp, 0, DAlarmFields, 0 },
	{ VCDataSizeProp, 0, 0, PD_INTERNAL },
	{ VCDayLightProp, 0, 0, 0 },
	//
	{ VCDCreatedProp, 0, 0, 0 },
	{ VCDeliveryLabelProp, 0, 0, 0 },
	{ VCDescriptionProp, 0, 0, 0 },
	{ VCDIBProp, 0, 0, 0 },
	{ VCDisplayStringProp, 0, 0, 0 },
	{ VCDomesticProp, 0, 0, 0 },
	{ VCDTendProp, 0, 0, 0 },
	{ VCDTstartProp, 0, 0, 0 },
	{ VCDueProp, 0, 0, 0 },
	{ VCEmailAddressProp, 0, 0, 0 },
	{ VCEncodingProp, 0, 0, 0 },
	{ VCEndProp, 0, 0, 0 },
	//
	{ VCEventProp, 0, 0, PD_BEGIN },
	{ VCEWorldProp, 0, 0, 0 },
	{ VCExNumProp, 0, 0, 0 },
	{ VCExpDateProp, 0, 0, 0 },
	{ VCExpectProp, 0, 0, 0 },
	{ VCExtAddressProp, 0, 0, 0 },
	{ VCFamilyNameProp, 0, 0, 0 },
	{ VCFaxProp, 0, 0, 0 },
	{ VCFullNameProp, 0, 0, 0 },
	{ VCGeoLocationProp, 0, 0, 0 },
	{ VCGeoProp, 0, 0, 0 },
	{ VCGIFProp, 0, 0, 0 },
	{ VCGivenNameProp, 0, 0, 0 },
	{ VCGroupingProp, 0, 0, 0 },
	{ VCHomeProp, 0, 0, 0 },
	{ VCIBMMailProp, 0, 0, 0 },
	{ VCInlineProp, 0, 0, 0 },
	{ VCInternationalProp, 0, 0, 0 },
	{ VCInternetProp, 0, 0, 0 },
	{ VCISDNProp, 0, 0, 0 },
	{ VCJPEGProp, 0, 0, 0 },
	{ VCLanguageProp, 0, 0, 0 },
	{ VCLastModifiedProp, 0, 0, 0 },
	{ VCLastRevisedProp, 0, 0, 0 },
	{ VCLocationProp, 0, 0, 0 },
	{ VCLogoProp, 0, 0, 0 },
	{ VCMailerProp, 0, 0, 0 },
	{ VCMAlarmProp, 0, MAlarmFields, 0 },
	{ VCMCIMailProp, 0, 0, 0 },
	//
	{ VCMsgProp, 0, 0, 0 },
	{ VCMETProp, 0, 0, 0 },
	{ VCModemProp, 0, 0, 0 },
	{ VCMPEG2Prop, 0, 0, 0 },
	{ VCMPEGProp, 0, 0, 0 },
	{ VCMSNProp, 0, 0, 0 },
	{ VCNamePrefixesProp, 0, 0, 0 },
	{ VCNameProp, 0, nameFields, 0 },
	{ VCNameSuffixesProp, 0, 0, 0 },
	//
	{ VCNtProp, 0, 0, 0 },
	{ VCOrgNameProp, 0, 0, 0 },
	{ VCOrgProp, 0, orgFields, 0 },
	{ VCOrgUnit2Prop, 0, 0, 0 },
	{ VCOrgUnit3Prop, 0, 0, 0 },
	{ VCOrgUnit4Prop, 0, 0, 0 },
	{ VCOrgUnitProp, 0, 0, 0 },
	{ VCPagerProp, 0, 0, 0 },
	{ VCPAlarmProp, 0, PAlarmFields, 0 },
	{ VCParcelProp, 0, 0, 0 },
	{ VCPartProp, 0, 0, 0 },
	{ VCPCMProp, 0, 0, 0 },
	{ VCPDFProp, 0, 0, 0 },
	{ VCPGPProp, 0, 0, 0 },
	{ VCPhotoProp, 0, 0, 0 },
	{ VCPICTProp, 0, 0, 0 },
	{ VCPMBProp, 0, 0, 0 },
	{ VCPostalBoxProp, 0, 0, 0 },
	{ VCPostalCodeProp, 0, 0, 0 },
	{ VCPostalProp, 0, 0, 0 },
	{ VCPowerShareProp, 0, 0, 0 },
	{ VCPreferredProp, 0, 0, 0 },
	{ VCPriorityProp, 0, 0, 0 },
	{ VCProcedureNameProp, 0, 0, 0 },
	{ VCProdIdProp, 0, 0, 0 },
	{ VCProdigyProp, 0, 0, 0 },
	{ VCPronunciationProp, 0, 0, 0 },
	{ VCPSProp, 0, 0, 0 },
	{ VCPublicKeyProp, 0, 0, 0 },
	{ VCQPProp, VCQuotedPrintableProp, 0, 0 },
	{ VCQuickTimeProp, 0, 0, 0 },
	{ VCQuotedPrintableProp, 0, 0, 0 },
	{ VCRDateProp, 0, 0, 0 },
	{ VCRegionProp, 0, 0, 0 },
	{ VCRelatedToProp, 0, 0, 0 },
	{ VCRepeatCountProp, 0, 0, 0 },
	{ VCResourcesProp, 0, 0, 0 },
	{ VCRNumProp, 0, 0, 0 },
	{ VCRoleProp, 0, 0, 0 },
	{ VCRRuleProp, 0, 0, 0 },
	{ VCRSVPProp, 0, 0, 0 },
	{ VCRunTimeProp, 0, 0, 0 },
	{ VCSequenceProp, 0, 0, 0 },
	{ VCSnoozeTimeProp, 0, 0, 0 },
	{ VCStartProp, 0, 0, 0 },
	{ VCStatusProp, 0, 0, 0 },
	{ VCStreetAddressProp, 0, 0, 0 },
	{ VCSubTypeProp, 0, 0, 0 },
	{ VCSummaryProp, 0, 0, 0 },
	{ VCTelephoneProp, 0, 0, 0 },
	{ VCTIFFProp, 0, 0, 0 },
	{ VCTimeZoneProp, 0, 0, 0 },
	{ VCTitleProp, 0, 0, 0 },
	{ VCTLXProp, 0, 0, 0 },
	{ VCTodoProp, 0, 0, PD_BEGIN },
	{ VCTranspProp, 0, 0, 0 },
	{ VCUniqueStringProp, 0, 0, 0 },
	{ VCURLProp, 0, 0, 0 },
	{ VCURLValueProp, 0, 0, 0 },
	{ VCValueProp, 0, 0, 0 },
	{ VCVersionProp, 0, 0, 0 },
	{ VCVideoProp, 0, 0, 0 },
	{ VCVoiceProp, 0, 0, 0 },
	{ VCWAVEProp, 0, 0, 0 },
	{ VCWMFProp, 0, 0, 0 },
	{ VCWorkProp, 0, 0, 0 },
	{ VCX400Prop, 0, 0, 0 },
	{ VCX509Prop, 0, 0, 0 },
	{ VCXRuleProp, 0, 0, 0 },
	/*------------FOR VNOTE--------------*/
	{ VLUIDProp,0,0,0 },
	{ VCEnvProp,0,0,0 },
	{ VCNoteProp, 0, 0, PD_BEGIN },
	/*-----------------------------------*/

	/*----------- for VMSG---------------*/
	{ VCMessageProp, 0, 0, PD_BEGIN },
	{ VCVEnvProp,0,0,PD_BEGIN },
	{ VCVBodyProp, 0, 0, PD_BEGIN },
	{ VCFromProp, 0, 0, 0 },
	{ VCToProp,0,0,0 },
	{ VCSubjectProp, 0, 0, 0 },
	{ VCMsgBodyContentProp,0,0,0 },
	/*-----------------------------------*/
	{ 0,0,0,0 }
};


static struct PreDefProp* lookupPropInfo(const char* str)
{
	/* brute force for now, could use a hash table here. */
	int i;

	for (i = 0; propNames[i].name; i++)
		if (stricmp(str, propNames[i].name) == 0) {
			return &propNames[i];
		}

		return 0;
}


DLLEXPORT(const char*) lookupProp_(const char* str)
{
	int i;

	for (i = 0; propNames[i].name; i++)
		if (stricmp(str, propNames[i].name) == 0) {
			const char* s;
			s = propNames[i].alias?propNames[i].alias:propNames[i].name;
			return lookupStr(s);
		}
		return lookupStr(str);
}


DLLEXPORT(const char*) lookupProp(const char* str)
{
	int i;

	for (i = 0; propNames[i].name; i++)
		if (stricmp(str, propNames[i].name) == 0) {
			const char *s;
			/*  const char** fields; */
			fieldedProp = propNames[i].fields;
			s = propNames[i].alias?propNames[i].alias:propNames[i].name;
			return lookupStr(s);
		}
		/* vobject.c(103):const char** fieldedProp; */
		fieldedProp = 0;
		return lookupStr(str);
}


/*----------------------------------------------------------------------
APIs to Output text form.
----------------------------------------------------------------------*/
#define OFILE_REALLOC_SIZE 512
typedef struct OFile {
	FILE *fp;
	char *s;
	int len;
	int limit;
	int alloc:1;
	int fail:1;
} OFile;

#if 0 /* for what? */
static void appendsOFile(OFile *fp, const char *s)
{/* s means string */
	int slen;
	if (fp->fail) return;
	slen  = strlen(s);
	if (fp->fp) {
		fwrite(s,1,slen,fp->fp);
	}
	else {
stuff:
		if (fp->len + slen < fp->limit) {
			memcpy(fp->s+fp->len,s,slen);
			fp->len += slen;
			return;
		}
		else if (fp->alloc) {
			fp->limit = fp->limit + OFILE_REALLOC_SIZE;
			if (OFILE_REALLOC_SIZE <= slen) fp->limit += slen;
			fp->s = (char *) realloc(fp->s,fp->limit);
			if (fp->s) goto stuff;
		}
		if (fp->alloc)
			free(fp->s);
		fp->s = 0;
		fp->fail = 1;
	}
}

static void appendcOFile(OFile *fp, char c)
{/* c means char */
	if (fp->fail) return;
	if (fp->fp) {
		fputc(c,fp->fp);
	}
	else {
stuff:
		if (fp->len+1 < fp->limit) {
			fp->s[fp->len] = c;
			fp->len++;
			return;
		}
		else if (fp->alloc) {
			fp->limit = fp->limit + OFILE_REALLOC_SIZE;
			fp->s = (char *) realloc(fp->s,fp->limit);
			if (fp->s) goto stuff;
		}
		if (fp->alloc)
			free(fp->s);
		fp->s = 0;
		fp->fail = 1;
	}
}
#else /* for what? */
static void appendcOFile_(OFile *fp, char c)
{
	if (fp->fail) return;
	if (fp->fp) {
		fputc(c,fp->fp);
	}
	else {
stuff:
		if (fp->len+1 < fp->limit) {
			fp->s[fp->len] = c;
			fp->len++;
			return;
		}
		else if (fp->alloc) {
			fp->limit = fp->limit + OFILE_REALLOC_SIZE;
			fp->s = realloc(fp->s,fp->limit);
			if (fp->s) goto stuff;
		}
		if (fp->alloc)
			free(fp->s);
		fp->s = 0;
		fp->fail = 1;
	}
}

/*static void appendcOFile(OFile *fp, char c)
{
Delete on 2003.03.05, there is not need to let upper app to call this.
if (c == '\n') {
/* write out as <CR><LF> 
appendcOFile_(fp,0xd);
appendcOFile_(fp,0xa);
}
else
appendcOFile_(fp,c);
}*/

static void appendsOFile(OFile *fp, const char *s)
{
	int i, slen;
	slen  = strlen(s);
	for (i=0; i<slen; i++) {
		appendcOFile_(fp,s[i]);
	}
}

#endif /* for what? */

static void initOFile(OFile *fp, FILE *ofp)
{
	fp->fp = ofp;
	fp->s = 0;
	fp->len = 0;
	fp->limit = 0;
	fp->alloc = 0;
	fp->fail = 0;
}

static void initMemOFile(OFile *fp, char *s, int len)
{
	fp->fp = 0;
	fp->s = s;
	fp->len = 0;
	fp->limit = s?len:0;
	fp->alloc = s?0:1;
	fp->fail = 0;
}


static int writeBase64(OFile *fp, unsigned char *s, long len)
{
	long cur = 0;
	int i, numQuads = 0;
	unsigned long trip;
	unsigned char b;
	char quad[5];
#define MAXQUADS 16

	quad[4] = 0;

	while (cur < len) {
		// collect the triplet of bytes into 'trip'
		trip = 0;
		for (i = 0; i < 3; i++) {
			b = (cur < len) ? *(s + cur) : 0;
			cur++;
			trip = trip << 8 | b;
		}
		// fill in 'quad' with the appropriate four characters
		for (i = 3; i >= 0; i--) {
			b = (unsigned char)(trip & 0x3F);
			trip = trip >> 6;
			if ((3 - i) < (cur - len))
				quad[i] = '='; // pad char
			else if (b < 26) quad[i] = (char)b + 'A';
			else if (b < 52) quad[i] = (char)(b - 26) + 'a';
			else if (b < 62) quad[i] = (char)(b - 52) + '0';
			else if (b == 62) quad[i] = '+';
			else quad[i] = '/';
		}
		// now output 'quad' with appropriate whitespace and line ending
		appendsOFile(fp, (numQuads == 0 ? "    " : ""));
		appendsOFile(fp, quad);
		appendsOFile(fp, ((cur >= len)?"\r\n" :(numQuads==MAXQUADS-1?"\r\n" : "")));
		numQuads = (numQuads + 1) % MAXQUADS;
	}
	appendsOFile(fp,"\r\n");

	return 1;
}

static void writeQPString(OFile *fp, const char *s)
{
	const char *p = s;
	while (*p) {
		if (*p == '\r'&& *(p+1)=='\n') {
			appendsOFile(fp,"=0D=0A");
			p+=2;
			continue;
		}
		appendcOFile_(fp,*p);
		p++;
	}
}

static void writeVObject_(OFile *fp, VObject *o);

static void writeValue(OFile *fp, VObject *o, unsigned long size)
{
	if (o == 0) return;
	switch (VALUE_TYPE(o)) {
case VCVT_USTRINGZ: {
	char *s = fakeCString(USTRINGZ_VALUE_OF(o));
	writeQPString(fp,s);
	deleteStr(s);
	break;
					}
case VCVT_STRINGZ: {
	writeQPString(fp, STRINGZ_VALUE_OF(o));
	break;
				   }
case VCVT_UINT: {
	char buf[16];
	sprintf(buf,"%u", INTEGER_VALUE_OF(o));
	appendsOFile(fp,buf);
	break;
				}
case VCVT_ULONG: {
	char buf[16];
	sprintf(buf,"%lu", LONG_VALUE_OF(o));
	appendsOFile(fp,buf);
	break;
				 }
case VCVT_RAW: {
	appendsOFile(fp,"\r\n");
	writeBase64(fp,(unsigned char*)(ANY_VALUE_OF(o)),size);
	break;
			   }
case VCVT_VOBJECT:
	appendsOFile(fp,"\r\n");
	writeVObject_(fp,VOBJECT_VALUE_OF(o));
	break;
	}
}

VObject* CopyValue(VObject* o, VObject* a, int size)
{
	if (o == 0 || a == 0) return NULL;
	switch (VALUE_TYPE(a)) {
		case VCVT_USTRINGZ: {
		setVObjectUStringZValue(o, USTRINGZ_VALUE_OF(a));
		break;
						}
		case VCVT_STRINGZ: {
		setVObjectStringZValue(o, STRINGZ_VALUE_OF(a));
		break;
					   }
		case VCVT_UINT: {
		setVObjectIntegerValue(o, INTEGER_VALUE_OF(a));
		break;
					}
		case VCVT_ULONG: {
		setVObjectLongValue(o, INTEGER_VALUE_OF(a));
		break;
					 }
		case VCVT_RAW: {
		/*char* buf = (char*)malloc(sizeof(char) * size);
		memcpy(buf, ANY_VALUE_OF(a), size);
		setVObjectAnyValue(o, buf);
		LONG_VALUE_OF(o) = size;*/
		setValueWithSize(o, ANY_VALUE_OF(a), size);
		break;
				   }
		case VCVT_VOBJECT:
		//appendsOFile(fp,"\r\n");
		//writeVObject_(fp,VOBJECT_VALUE_OF(o));
		break;
	}
	return a;
}

static void writeAttrValue(OFile *fp, VObject *o)
{
	if (NAME_OF(o)) {
		struct PreDefProp *pi;
		pi = lookupPropInfo(NAME_OF(o));
		if (pi && ((pi->flags & PD_INTERNAL) != 0)) return;
		appendcOFile_(fp,';');
		appendsOFile(fp,NAME_OF(o));
	}
	else
		appendcOFile_(fp,';');
	if (VALUE_TYPE(o)) {
		appendcOFile_(fp,'=');
		writeValue(fp,o,0);
	}
}

VObject* CopyAttrValue(VObject* o, VObject* a)
{
	if (NAME_OF(a)) {
		struct PreDefProp *pi;
		pi = lookupPropInfo(NAME_OF(a));
		if (pi && ((pi->flags & PD_INTERNAL) != 0))
			return NULL;
	}

	setVObjectName(o, NAME_OF(a));

	if (VALUE_TYPE(a)) {
		return CopyValue(o, a, 0);
	} else {
		return NULL;
	}
}


static void writeGroup(OFile *fp, VObject *o)
{
	//..?? safe?
	char buf1[512];
	char buf2[512];
	strcpy(buf1,NAME_OF(o));
	while ((o=isAPropertyOf(o,VCGroupingProp)) != 0) {
		strcpy(buf2,STRINGZ_VALUE_OF(o));
		strcat(buf2,".");
		strcat(buf2,buf1);
		strcpy(buf1,buf2);
	}
	appendsOFile(fp,buf1);
}

VObject* CopyGrop(VObject* o, VObject* g)
{
	if (o && g) {
		while ((g=isAPropertyOf(g,VCGroupingProp)) != 0) {
			addGroup(o, STRINGZ_VALUE_OF(g));
		}
	}
	return o;
}

static int inList(const char **list, const char *s)
{
	if (list == 0) return 0;
	while (*list) {
		if (stricmp(*list,s) == 0) return 1;
		list++;
	}
	return 0;
}

static void writeProp(OFile *fp, VObject *o)
{
	if (NAME_OF(o)) {
		struct PreDefProp *pi;
		VObjectIterator t;
		const char **fields_ = 0;
		pi = lookupPropInfo(NAME_OF(o));
		if (pi && ((pi->flags & PD_BEGIN) != 0)) {
			writeVObject_(fp,o);
			return;
		}
		if (isAPropertyOf(o,VCGroupingProp))
			writeGroup(fp,o);
		else
			appendsOFile(fp,NAME_OF(o));
		if (pi) fields_ = pi->fields;
		initPropIterator(&t,o,NULL);
		while (moreIteration(&t)) {
			const char *s;
			VObject *eachProp = nextVObject(&t);
			s = NAME_OF(eachProp);
			if (stricmp(VCGroupingProp,s) && !inList(fields_,s))
				writeAttrValue(fp,eachProp);
		}
		if (fields_) {
			int i = 0, n = 0;
			const char** fields = fields_;
			/* output prop as fields */
			if (fp->s[fp->len] != ':') {
				appendcOFile_(fp,':');
			}
//			appendcOFile_(fp,':');
			while (*fields) {
				VObject *t = isAPropertyOf(o,*fields);
				i++;
				if (t) n = i;
				fields++;
			}
			fields = fields_;
			for (i=0;i<n;i++) {
				writeValue(fp,isAPropertyOf(o,*fields),0);
				fields++;
				if (i<(n-1)) appendcOFile_(fp,';');
			}
		}
	}

	if (VALUE_TYPE(o)) {
		unsigned long size = 0;
		VObject *p = isAPropertyOf(o,VCDataSizeProp);
		if (p) size = LONG_VALUE_OF(p);
		if (fp->s[fp->len - 1] != ':') {
			appendcOFile_(fp,':');
		}
		writeValue(fp,o,size);
	}

	appendsOFile(fp,"\r\n");
}

static void writeVObject_(OFile *fp, VObject *o)
{
	if (NAME_OF(o)) {
		struct PreDefProp *pi;
		pi = lookupPropInfo(NAME_OF(o));

		if (pi && ((pi->flags & PD_BEGIN) != 0)) {
			VObjectIterator t;
			const char *begin = NAME_OF(o);
			appendsOFile(fp,"BEGIN:");
			appendsOFile(fp,begin);
			appendsOFile(fp,"\r\n");
			initPropIterator(&t,o,NULL);
			while (moreIteration(&t)) {
				VObject *eachProp = nextVObject(&t);
				writeProp(fp, eachProp);
			}
			appendsOFile(fp,"END:");
			appendsOFile(fp,begin);
			appendsOFile(fp,"\r\n");
			/*============================------------------==========================*/
		}
	}
}
/*
void writeVObject(FILE *fp, VObject *o);
-- convert VObject o to its textual representation and
write it to file.
*/
void writeVObject(FILE *fp, VObject *o)
{
	OFile ofp;
	_setmode(_fileno(fp), _O_BINARY);
	initOFile(&ofp,fp);
	writeVObject_(&ofp,o);
}

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to write a property to the
Arguments:
	VObject* o
	int* len
Return:
	char*
---------------------------------------------------------------------------*/
DLLEXPORT(char*) writeProperty(VObject* o, int* len)
{
	OFile ofp;
	initMemOFile(&ofp, NULL, 0);
	writeProp(&ofp,o);
	if (len) *len = ofp.len;
	appendcOFile_(&ofp,0);
	*len = ofp.len;
	return ofp.s;
}

DLLEXPORT(void) writeVObjectToFile(char *fname, VObject *o)
{
	FILE *fp = fopen(fname,"w");
	if (fp) {
		writeVObject(fp,o);
		fclose(fp);
	}
}

DLLEXPORT(void) writeVObjectToFileWithFilters(char *fname, VObject *o, char* filters)
{
	FILE *fp = fopen(fname,"w");
	if (fp) {
		writeVObjectWithFilters(fp,o, filters);
		fclose(fp);
	}
}

void writeVObjectWithFilters(FILE *fp, VObject *o, char* filters)
{
	OFile ofp;
	_setmode(_fileno(fp), _O_BINARY);
	initOFile(&ofp,fp);
	writeVObjectWithFilters_(&ofp,o, filters);
}

void writeVObjectWithFilters_(void *fp, VObject *o, char* filters)
{
	if (NAME_OF(o)) {
		struct PreDefProp *pi;
		pi = lookupPropInfo(NAME_OF(o));

		if (pi && ((pi->flags & PD_BEGIN) != 0)) {
			VObjectIterator t;
			const char *begin = NAME_OF(o);
			appendsOFile(fp,"BEGIN:");
			appendsOFile(fp,begin);
			appendsOFile(fp,"\r\n");
			initPropIterator(&t,o,NULL);
			while (moreIteration(&t)) {
				VObject *eachProp = nextVObject(&t);
/*				if (isFilterSigned((char*)eachProp->id, filters)) {*/
					writeProp(fp, eachProp);
				/*}*/
			}
			appendsOFile(fp,"END:");
			appendsOFile(fp,begin);
			appendsOFile(fp,"\r\n");
			//appendsOFile(fp,"\n\n");
			/*============================------------------==========================*/
		}
	}
}
DLLEXPORT(void) writeVObjectsToFile(char *fname, VObject *list)
{
	FILE *fp = fopen(fname,"w");
	if (fp) {
		while (list) {
			writeVObject(fp,list);
			list = nextVObjectInList(list);
		}
		fclose(fp);
	}
}

/******** used for VCMsgBodyContentProp (vmessage) **********/
DLLEXPORT(char *) getMsgBdCntFromFile(char *fname)
{	
	FILE *fp = fopen(fname,"r");
	if(fp){
		char *buffer,*pbegin,*pend,*p,*pdest,*ch;
		unsigned int count = 0;
		while (!feof(fp)){
			fgetc(fp); 
			count++;
		}
		buffer = (char*)malloc((count)*sizeof(char));	
		p = buffer;
		rewind(fp);
		while (!feof(fp)){
			*p = fgetc(fp); 
			p++;
		}

		ch = "BEGIN:VBODY";
		pbegin = strstr(buffer,ch);
		if(!pbegin){
			free(buffer);
			return (char *)0;
		}

		ch = "END:VBODY";
		pend = strstr(buffer,ch);
		if(!pend){
			free(buffer);
			return (char *)0;
		}
		/*in text mode ,it is 0A,but in binary modeie it is oA oD two characters!*/
		p = pend-1;	
		*p = 0;		
		/* now,it's the last character!*/
		p--;		
		while(*p!=':' && p!=pbegin){
			p--;
		}
		while(*p!='\n' && *p!='\r' && *p!='\0' && p!=pend){
			p++;
		}
		p++;
		/*
		while(*p!='\n' && *p!='\r' && *p!='\0' && p!=pbegin){
		p--;
		}
		p++;
		*/	
		/* skip over blanks*//*int isspace(ch)*/
		while(*p == ' '){
			p++;
		}
		/*pay attention to this pointer should be deallocated by deleteStr()*/
		pdest = dupStr(p,0); 
		free(buffer);
		fclose(fp);
		return pdest;
	}
	else 
		return (char *)0;
}


DLLEXPORT(int)  putMsgBdCntToFile(char *fname,char* str)
{	FILE *fp = fopen(fname,"r");
if(fp){
	char *buffer,*pbegin,*pend,*p,*ch;
	unsigned int size =0,rest =0,count = 0;
	while (!feof(fp)){
		fgetc(fp); 
		count++;
	}
	/* skip over blanks*//*int isspace(ch)*/
	while (*str == ' ')
	{
		str++;
	}
	size = strlen(str);

	buffer = (char*)malloc((count+size)*sizeof(char));	
	p = buffer;
	rewind(fp);
	while (!feof(fp)){
		*p = fgetc(fp); 
		p++;
	}
	fclose(fp);

	ch = "BEGIN:VBODY";
	pbegin = strstr(buffer,ch);
	if(!pbegin){
		free(buffer);
		return 0;
	}

	ch = "END:VBODY";
	pend = strstr(buffer,ch);
	if(!pend){
		free(buffer);
		return 0;
	}
	/* p,now, is the positon that the Message-Body-Content should be inserted into!*/
	p = pend;

	while (pend != buffer + count - 1){
		pend++,rest++;
	}

	//void *memmove( void *dest, const void *src, size_t count );

	memmove(buffer+count+size-rest,p,rest);   
	memcpy(p,str,size);	
	*(buffer+count+size-rest-1) = '\n';
	fp = fopen(fname,"w");
	fwrite(buffer,sizeof(char),(count+size)*sizeof(char),fp);
	free(buffer);
	fclose(fp);
	return 1;
}
else 
return  0;

}
/*************************************************************/

/*
char* writeMemVObject(char *s, int *len, VObject *o);
-- convert VObject o to its textual representation and
write it to memory. If s is 0, then memory required
to hold the textual representation will be allocated
by this API. If a variable len is passed, len will
be overwriten with the byte size of the textual
representation. If s is non-zero, then s has to
be a user allocated buffer whose size has be passed
in len as a variable. Memory allocated by the API
has to be freed with call to free. The return value
of this API is either the user supplied buffer,
the memory allocated by the API, or 0 (in case of
failure).
*/
DLLEXPORT(char*) writeMemVObject(char *s, int *len, VObject *o)
{
	OFile ofp;
	initMemOFile(&ofp,s,len?*len:0);
	writeVObject_(&ofp,o);
	if (len) *len = ofp.len;
	appendcOFile_(&ofp,0);
	return ofp.s;
}

DLLEXPORT(char*) writeMemVObjects(char *s, int *len, VObject *list)
{
	OFile ofp;
	initMemOFile(&ofp,s,len?*len:0);
	while (list) {
		writeVObject_(&ofp,list);
		list = nextVObjectInList(list);
	}
	if (len) *len = ofp.len;
	appendcOFile_(&ofp,0);
	return ofp.s;
}

/*----------------------------------------------------------------------
APIs to do fake Unicode stuff.
----------------------------------------------------------------------*/
/*
wchar_t* fakeUnicode(const char *ps, int *bytes);
-- convert char* to wchar_t*.
*/
DLLEXPORT(wchar_t*) fakeUnicode(const char *ps, int *bytes)
{
	wchar_t *r, *pw;
	int len = strlen(ps)+1;

	pw = r = (wchar_t*)malloc(sizeof(wchar_t)*len);
	if (bytes)
		*bytes = len * sizeof(wchar_t);

	while (*ps) { 
		if (*ps == '\n')
			*pw = (wchar_t)0x2028;
		else if (*ps == '\r')
			*pw = (wchar_t)0x2029;
		else
			*pw = (wchar_t)(unsigned char)*ps;
		ps++; pw++;
	}				 
	*pw = (wchar_t)0;

	return r;
}
/*
extern int uStrLen(const wchar_t *u);
-- length of unicode u.

wchar_t: internal type of a wide character
Useful for writing portable programs for international markets. 
include in STDDEF.H, STDLIB.H
*/
DLLEXPORT(int) uStrLen(const wchar_t *u)
{
	int i = 0;
	while (*u != (wchar_t)0) { u++; i++; }
	return i;
}
/*
char *fakeCString(const wchar_t *u);
-- convert wchar_t to CString (blindly assumes that
this could be done).
*/
DLLEXPORT(char*) fakeCString(const wchar_t *u)
{
	char *s, *t;
	int len = uStrLen(u) + 1;
	/* for \0 */
	t = s = (char*)malloc(len);
	while (*u) {
		if (*u == (wchar_t)0x2028)
			*t = '\n';
		else if (*u == (wchar_t)0x2029)
			*t = '\r';
		else
			*t = (char)*u;
		u++; t++;
	}
	*t = 0;
	return s;
}

// end of source file vobject.c
/*
void deleteStr(const char *p);
-- used to deallocate a string allocated by dupStr();
*/
/*DLLEXPORT(void) deleteStr(const char *p)
{
if (p) free((void*)p);
}
*/
/*
DLLEXPORT(char*) dupStr(const char *s, unsigned int size)
{
char *t;
if  (size == 0) {
size = strlen(s);
}
t = (char*)malloc(size+1);
if (t) {
memcpy(t,s,size);
t[size] = 0;

return t;
}
else {
return (char*)0;
}
}
*/

/*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Description:
	This function is to copy the property to a new VObject.
Arguments:
	VObject *o
Return:
	VObject* 
---------------------------------------------------------------------------*/
DLLEXPORT(VObject*) CopyProperty(VObject *o)
{
	VObject* newobj = newVObject(NAME_OF(o));
	if (o && NAME_OF(o)) {
		struct PreDefProp *pi;
		VObjectIterator t;
		const char **fields_ = 0;
		pi = lookupPropInfo(NAME_OF(o));
		
		if (pi && ((pi->flags & PD_BEGIN) != 0)) {
			/*writeVObject_(fp,o);*/
			//this consumed not to appear.
			cleanVObject(newobj);
			return NULL;
		}
		
		/*if (isAPropertyOf(o,VCGroupingProp))
			writeGroup(fp,o);
		else
			appendsOFile(fp,NAME_OF(o));
		*/

		if (isAPropertyOf(o,VCGroupingProp)) {
			CopyGrop(newobj, o);
		}

		if (pi) fields_ = pi->fields;
		initPropIterator(&t,o,NULL);
		while (moreIteration(&t)) {
			const char *s;
			VObject *eachProp = nextVObject(&t);
			VObject *neweachprop = newVObject(NAME_OF(eachProp));
			s = NAME_OF(eachProp);
			if (strcmp(s, VCDataSizeProp) != 0) {
				addVObjectProp(newobj, neweachprop);
				if (stricmp(VCGroupingProp,s) && !inList(fields_,s))
					CopyAttrValue(neweachprop, eachProp);
			}
		}
		if (fields_) {
			int i = 0, n = 0;
			const char** fields = fields_;
			/* output prop as fields */
			//appendcOFile_(fp,':');
			while (*fields) {
				VObject *t = isAPropertyOf(o,*fields);
				i++;
				if (t) n = i;
				fields++;
			}
			fields = fields_;
			for (i=0;i<n;i++) {
				CopyValue(newobj, isAPropertyOf(o,*fields), 0);
				fields++;
				//if (i<(n-1)) appendcOFile_(fp,';');
			}
		}
	}

	if (VALUE_TYPE(o)) {
		unsigned long size = 0;
		VObject *p = isAPropertyOf(o, VCDataSizeProp);
		if (p) {
			size = LONG_VALUE_OF(p);
		}
		//appendcOFile_(fp,':');
		CopyValue(newobj, o, size);
	}
	return newobj;
}

DLLEXPORT(char*) GetProperty(char* g, char* id, int* len)
{
	VObject* o = (VObject*)g;
	VObject * p = NULL;
	VObject* n = NULL;
	char* mem = NULL;
	if ((p =isAPropertyOf(o, id)) != NULL) {
		n = CopyProperty(p);
		mem = writeProperty(n, len);
		cleanVObject(n);
	}
	return mem;
}

DLLEXPORT(char*) FindFirstProperty(char* g, char* id, char **find_hdl, int* len)
{
	VObject* o = (VObject*)g;
	VObjectIterator *i;
	VObject *p = NULL;
	char* mem = NULL;

	i = malloc(sizeof(VObjectIterator));
	initPropIterator(i, o, id);
	p = nextVObject(i);
	while (p != NULL) {
		if (id == NULL || !stricmp(id, p->id)) {
			VObject* n = NULL;
			n = CopyProperty(p);
			mem = writeProperty(n, len);
			cleanVObject(n);
			break;
		}
		p = nextVObject(i);
	}
	if (find_hdl != NULL)
		*find_hdl = (char*)i;
	return mem;
}

DLLEXPORT(char*) FindNextProperty(char* find_hdl, int* len)
{
	VObjectIterator *i = (VObjectIterator*)find_hdl;
	VObject *p = NULL;
	char* mem = NULL;

	if (i == NULL)
		return NULL;
	p = nextVObject(i);
	while (p != NULL) {
		if (i->id == NULL || !stricmp(i->id, p->id)) {
			VObject* n = NULL;
			n = CopyProperty(p);
			mem = writeProperty(n, len);
			cleanVObject(n);
			break;
		}
		p = nextVObject(i);
	}
	return mem;
}

DLLEXPORT(void) EndFindProperty(char* find_hdl)
{
	if (find_hdl != NULL) {
		VObjectIterator *i = (VObjectIterator*)find_hdl;
		if (i->id != NULL) {
			free(i->id);
			i->id = NULL;
		}
		free(i);
	}
}

DLLEXPORT(void) FreevCardBuff(char* buf)
{
 	if (buf) {
 		free(buf);
 	}
}

DLLEXPORT(char*) vCardParse(int file_hdl)
{
	return (char*)Parse_MIME_FromFile((FILE*)file_hdl);
}

DLLEXPORT(void) vCardClose(char* o)
{
	cleanVObject((VObject*)o);
	FinishParse();
	cleanStrTbl();
}