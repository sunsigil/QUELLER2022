using System.Collections;
using System.Collections.Generic;

public interface ISavable
{
	void LoadDefaults();
	string WriteBlock();
	bool ReadBlock(string block);
}
