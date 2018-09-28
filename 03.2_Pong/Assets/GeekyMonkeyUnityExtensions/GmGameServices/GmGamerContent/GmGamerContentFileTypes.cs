using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

namespace GeekyMonkey
{
    /// <summary>
    /// Mime Type Keys
    /// </summary>
    public enum GmGamerContentFileType
    {
        autoFromFileName = 0,
        jpg,
        png,
        gif,
        json,
        xml,
        text,
        binary
    }
}
