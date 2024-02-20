using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pax
{
    public interface IPanelController
    {
        public void OpenPanel(EventArgs args);
        public void ClosePanel(EventArgs args);
    }
}

