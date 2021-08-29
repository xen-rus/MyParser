using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiParcer.Interfaces
{
    interface IParser
    {
        void Parce();
        Task<bool> Connect();
        int GetWordCount();
        List<string> GetChaildLinks();


    }
}
