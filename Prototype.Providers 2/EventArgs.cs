using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Providers {
    public class EventArgsCustom {

        public Guid Guid { get; set; }
        public Logging Logging { get; set; }
        public object Object { get; set; }
        public object Result { get; set; }
        public string Description { get; set; }

        public EventArgsCustom(Logging _logging)
            : this(_logging, null) { }

        public EventArgsCustom(Logging _logging, object _object)
            : this(_logging, _object, null) { }

        public EventArgsCustom(Logging _logging, object _object, object _result) {
            Logging = _logging;
            Object = _object;
            Result = _result;
        }
        public EventArgsCustom(object _object)
            : this(_object, null) { }

        public EventArgsCustom(object _object, object _result) {
            Logging = null;
            Object = _object;
            Result = _result;
        }
        public EventArgsCustom(Guid _id) {
            Guid = _id;
            Logging = null;
            Object = null;
            Result = null;
        }
        public EventArgsCustom(Guid _id, object _result) {
            Guid = _id;
            Logging = null;
            Object = null;
            Result = _result;
        }
        public EventArgsCustom(Guid _id, Logging _logging, object _object, object _result, string _description) {
            Guid = _id;
            Logging = _logging;
            Object = _object;
            Result = _result;
            Description = _description;
        }
    }
    public delegate void EventHandler(object sender, Providers.EventArgsCustom e);
}