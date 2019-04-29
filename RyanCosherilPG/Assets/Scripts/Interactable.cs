using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

interface Interactable
    {
         void interact(GameObject interactor);
         bool isInteractable();
    }

