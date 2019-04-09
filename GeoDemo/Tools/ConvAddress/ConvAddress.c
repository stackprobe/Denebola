#include "C:\Factory\Common\all.h"
#include "C:\Factory\Common\Options\csvStream.h"

#define R_DIR "C:\\wb\\�S���Z���ܓx�o�x"
#define W_DIR "C:\\wb\\�S���Z���ܓx�o�xT"

#define COLIDX_LEVEL_01 0 // "�s���{����"
#define COLIDX_LEVEL_02 1 // "�s�撬����"
#define COLIDX_LEVEL_03 2 // "�厚�E���ږ�"
#define COLIDX_LEVEL_04 3 // "�����E�ʏ̖�"
#define COLIDX_LEVEL_05 4 // "�X�敄���E�n��"
#define COLIDX_LAT 8      // "�ܓx"
#define COLIDX_LON 9      // "�o�x"

#define ADDRESS_TOKEN_LENMAX 30

static void CheckAddressToken(char *token, uint lenmin)
{
	uint len;

	errorCase(!token); // 2bs

	len = strlen(token);

	errorCase(!m_isRange(len, lenmin, ADDRESS_TOKEN_LENMAX));
	errorCase(!isJLine(token, 1, 0, 0, 0));
}
static void ConvAddress_Record(char *todoufuken, char *shikuchouson, char *ooazachoume, char *alias, char *chiban, char *sLat, char *sLon)
{
	char region[6];

	CheckAddressToken(todoufuken,   1);
	CheckAddressToken(shikuchouson, 1);
	CheckAddressToken(ooazachoume,  1);
	CheckAddressToken(alias,        0);
	CheckAddressToken(chiban,       1);

	errorCase(!sLat); // 2bs
	errorCase(!sLon); // 2bs

	errorCase(!lineExp("<1,19><1,09>.<6,09>", sLat)); //  10.000000 �`  99.999999
	errorCase(!lineExp("<1,19><2,09>.<6,09>", sLon)); // 100.000000 �` 999.999999

	region[0] = sLat[0];
	region[1] = sLat[1];
	region[2] = sLon[0];
	region[3] = sLon[1];
	region[4] = sLon[2];
	region[5] = '\0';

	{
		char *file = xcout("%s\\%s.conved.txt", W_DIR, region);
		FILE *fp;

		fp = fileOpen(file, "at");

		writeLine(fp, "A");
		writeLine(fp, todoufuken);
		writeLine(fp, shikuchouson);
		writeLine(fp, ooazachoume);
		writeLine(fp, alias);
		writeLine(fp, chiban);
		writeLine(fp, sLat);
		writeLine(fp, sLon);
		writeLine(fp, "/");

		fileClose(fp);
		memFree(file);
	}
}
static void ConvAddress_File(char *file)
{
	FILE *fp = fileOpen(file, "rt");

	cout("file: %s\n", file);

	{
		autoList_t *header = readCSVRow(fp);

		errorCase(!header);

		errorCase(strcmp(getLine(header, COLIDX_LEVEL_01), "�s���{����"));
		errorCase(strcmp(getLine(header, COLIDX_LEVEL_02), "�s�撬����"));
		errorCase(strcmp(getLine(header, COLIDX_LEVEL_03), "�厚�E���ږ�"));
		errorCase(strcmp(getLine(header, COLIDX_LEVEL_04), "�����E�ʏ̖�"));
		errorCase(strcmp(getLine(header, COLIDX_LEVEL_05), "�X�敄���E�n��"));
		errorCase(strcmp(getLine(header, COLIDX_LAT),      "�ܓx"));
		errorCase(strcmp(getLine(header, COLIDX_LON),      "�o�x"));

		releaseDim(header, 1);
	}

	LOGPOS();

	for(; ; )
	{
		autoList_t *row = readCSVRow(fp);

		if(!row)
			break;

		ConvAddress_Record(
			getLine(row, COLIDX_LEVEL_01),
			getLine(row, COLIDX_LEVEL_02),
			getLine(row, COLIDX_LEVEL_03),
			getLine(row, COLIDX_LEVEL_04),
			getLine(row, COLIDX_LEVEL_05),
			getLine(row, COLIDX_LAT),
			getLine(row, COLIDX_LON)
			);

		releaseDim(row, 1);
	}

	LOGPOS();

	fileClose(fp);
}
static void ConvAddress(void)
{
	autoList_t *dirs;
	char *dir;
	uint dir_index;

	errorCase(!existDir(R_DIR));

	recurRemoveDirIfExist(W_DIR);
	createDir(W_DIR);

	errorCase(!existDir(W_DIR));

	dirs = lsDirs(R_DIR);

	foreach(dirs, dir, dir_index)
	{
		if(endsWithICase(dir, "a"))
		{
			autoList_t *files = lsFiles(dir);
			char *file;
			uint file_index;

			foreach(files, file, file_index)
			{
				if(!_stricmp("csv", getExt(file)))
				{
					ConvAddress_File(file);
				}
			}
			releaseDim(files, 1);
		}
	}
	releaseDim(dirs, 1);
}
int main(int argc, char **argv)
{
	ConvAddress();
}
