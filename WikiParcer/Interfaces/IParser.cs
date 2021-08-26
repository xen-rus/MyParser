using System;
using System.Collections.Generic;
using System.Text;

namespace WikiParcer.Interfaces
{
    interface IParser
    {
        void Route(string link);

        void ParceAsync();
        int GetWordCount();
        List<string> GetChaildLinks();


    }
}
