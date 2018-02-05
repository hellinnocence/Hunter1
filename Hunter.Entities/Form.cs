﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{

    [BsonIgnoreExtraElements]
    public class Form : Entity
    {

        public string Name { get; set; }

        public string Html { get; set; }

        public string Remark { get; set; }

        public List<Field> Fields { get; set; }

        public List<Dictionary<string, object>> Columns { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Line> Lines { get; set; }

        public List<Area> Areas { get; set; }

        

        public class Field
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public override int GetHashCode()
            {
                if (this == null)
                    return 0;
                if (this.Name == null)
                    return 0;
                return this.Name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (this != null && obj is Field temp)
                    return this.Name == temp.Name;
                return false;
            }
        }


        public class Node
        {
            public string ID { get; set; }

            public bool Alt { get; set; }

            public int Top { get; set; }

            public int Left { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public string Name { get; set; }

            public string Type { get; set; }

            public List<string> Fields { get; set; }

            [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
            public bool IsEndType { get => Helper.IsEndTypeNode(this.Type); }

            [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
            public bool IsStartType { get => Helper.IsStartTypeNode(this.Type); }

        }

        public class Line
        {
            public string ID { get; set; }

            public double M { get; set; }

            public bool Alt { get; set; }

            public bool Marked { get; set; }

            public bool Dash { get; set; }

            public string Name { get; set; }

            public string From { get; set; }

            public string To { get; set; }

            public string Type { get; set; }

        }

        public class Area
        {
            public string ID { get; set; }

            public bool Alt { get; set; }

            public int Top { get; set; }

            public int Left { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public string Name { get; set; }

            public string Color { get; set; }
        }

    }
}
