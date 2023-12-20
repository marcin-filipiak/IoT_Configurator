using System;
using System.Collections.Generic;

namespace IoTConfigurator.Models
{
        public class Forms
        {
            public List<Form> forms { get; set; }

            public Forms(List<Form> forms)
            {
                this.forms = forms;
            }
        }
        public class Form
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public List<Member> Members { get; set; }
            public List<Button> Button { get; set; }
        }

        public class Member
        {
            public string Type { get; set; }
            public string Label { get; set; }
            public string Name { get; set; }
            public object Value { get; set; }
            public object Set { get; set; }
            public object Autosend { get; set; }

            // Add properties for other member types like 'set', 'autosend', 'values' as needed
        }

        public class Button
        {
            public string Type { get; set; }
            public string Label { get; set; }
            public string Name { get; set; }
        }

        public class ItemsList
        {
            public List<Item> Value;
        }

        public class Item
        {
            public string Value { get; set; }
            public string Label { get; set; }

        }
}

