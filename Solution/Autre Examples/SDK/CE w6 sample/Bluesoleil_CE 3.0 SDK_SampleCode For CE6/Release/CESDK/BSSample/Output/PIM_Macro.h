#ifndef _PIM_MACRO_H
#define _PIM_MACRO_H


#define PIM_CONNECT_OK				1	 //连接成功
#define PIM_CONNECT_FAIL 			2	 //连接失败
#define PIM_CONNECT_NEEDPATCH		3	 //需要上传patch
#define PIM_CONNECT_NEEDPHONEINFO	4	 //需要PHONEINFO

//SMS
#define PIM_SMS_BODY_LENGTH			256  // 短信最大body长度
#define PIM_PHONE_NUMBER_LENGTH	    41	 // 短信发送号码的最大长度
#define PIM_TIME_STAMP_LENGTH		32	 // 时间字符串的最大长度
#define MESSAGE_MAX_LENGTH			1000 // 保存短信的最大条数
//PB
#define PIM_CONTACT_NAME_LENGTH			32		    // Old:512 姓名字段的最大长度
#define PIM_CONTACT_URL_LENGTH			32		    // 图片链接最大长度
#define PIM_CONTACT_TELEPHONE_LENGTH	PIM_PHONE_NUMBER_LENGTH			// Old:128 电话字段的最大长度
#define PIM_CONTACT_ADDRESS_LENGTH		32			// 地址字段的最大长度
#define PIM_CONTACT_IM_LENGTH			32			// 聊天ID
#define PIM_CONTACT_BIRTHDAY_LENGTH		16			// Old:64 生日字段的最长
#define PIM_CONTACT_GROUP_LENGTH		32			// 群组最长
#define PIM_CONTACT_MEMO_LENGTH			32			// 备注最长
#define PIM_CONTACT_ID_LENGTH			64

#define PIM_SMS_INBOX			    0	 // 收件箱
#define PIM_SMS_OUTBOX			    1	 // 发件箱
#define PIM_SMS_TEMPLATE            3    // 模板

#define SMSSIM						111
#define SMSME						112

#define UNREAD						0
#define READ						1

//Service Type: PhoneBook, SMS or PhoneBook SMS both.
#define ST_PB                       1
#define ST_SMS                      2
#define ST_PB_SMS                   3

#endif