using ZenitV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.BL.Interfaces
{
    public interface IWordReader
    {
        enum FileType
        {
            SK42,
            PZ90,
            Unbind
        }

        FileType GetFileType();
        bool BindFile(string path);
        void UnbindFile();

        void InitForReadSK42();
        void InitForReadPZ();
        void AddColumnToRead(string name);
        void RemoveColumn(string name);
        bool ContainsColumn(string name);
        List<List<WordInputData>> MapFile();
    }
}
