using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [Serializable]
    public class ServingTask : BaseTask
    {
        // Implement abstract properties
        public override TaskType Type => TaskType.Serving;

        //Custom properties
        public GuestContext Guest => _guest;

        //Attributes
        [SerializeField] private GuestContext _guest;

        public ServingTask (Transform location, GuestContext guest)
        {
            _location = location;
            _isAssigned = false;
            _guest = guest;
        }

        public void SetGuest(GuestContext guest)
        {
            _guest = guest;
        }
    }
}