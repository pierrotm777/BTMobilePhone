#ifndef _PIM_MACRO_H
#define _PIM_MACRO_H


#define PIM_CONNECT_OK				1	 //���ӳɹ�
#define PIM_CONNECT_FAIL 			2	 //����ʧ��
#define PIM_CONNECT_NEEDPATCH		3	 //��Ҫ�ϴ�patch
#define PIM_CONNECT_NEEDPHONEINFO	4	 //��ҪPHONEINFO

//SMS
#define PIM_SMS_BODY_LENGTH			256  // �������body����
#define PIM_PHONE_NUMBER_LENGTH	    41	 // ���ŷ��ͺ������󳤶�
#define PIM_TIME_STAMP_LENGTH		32	 // ʱ���ַ�������󳤶�
#define MESSAGE_MAX_LENGTH			1000 // ������ŵ��������
//PB
#define PIM_CONTACT_NAME_LENGTH			32		    // Old:512 �����ֶε���󳤶�
#define PIM_CONTACT_URL_LENGTH			32		    // ͼƬ������󳤶�
#define PIM_CONTACT_TELEPHONE_LENGTH	PIM_PHONE_NUMBER_LENGTH			// Old:128 �绰�ֶε���󳤶�
#define PIM_CONTACT_ADDRESS_LENGTH		32			// ��ַ�ֶε���󳤶�
#define PIM_CONTACT_IM_LENGTH			32			// ����ID
#define PIM_CONTACT_BIRTHDAY_LENGTH		16			// Old:64 �����ֶε��
#define PIM_CONTACT_GROUP_LENGTH		32			// Ⱥ���
#define PIM_CONTACT_MEMO_LENGTH			32			// ��ע�
#define PIM_CONTACT_ID_LENGTH			64

#define PIM_SMS_INBOX			    0	 // �ռ���
#define PIM_SMS_OUTBOX			    1	 // ������
#define PIM_SMS_TEMPLATE            3    // ģ��

#define SMSSIM						111
#define SMSME						112

#define UNREAD						0
#define READ						1

//Service Type: PhoneBook, SMS or PhoneBook SMS both.
#define ST_PB                       1
#define ST_SMS                      2
#define ST_PB_SMS                   3

#endif