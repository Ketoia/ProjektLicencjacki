using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System;
using MessagePack.Resolvers;

namespace SaveSystem
{
    public static class SaveSystemSerializer
    {
        private static MessagePackSerializerOptions options;

        public static byte[] Serialize<T>(T item)
        {
            CheckOptions();
            return MessagePackSerializer.Serialize(item, options);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            CheckOptions();
            return MessagePackSerializer.Deserialize<T>(bytes, options);
        }

        private static void CheckOptions()
        {
            if(options == null)
            {
                options = new MessagePackSerializerOptions(CompositeResolver.Create(
                    StandardResolver.Instance, 
                    BuiltinResolver.Instance,
                    AttributeFormatterResolver.Instance,
                    GeneratedResolver.Instance,
                    MessagePack.Unity.UnityResolver.Instance));
            }
        }
    }

    [MessagePack.Union(0, typeof(test1))]
    [MessagePack.Union(1, typeof(test2))]
    [MessagePack.Union(2, typeof(SaveInt))]
    public class SaveData
    {
        [Key(0)]
        public Guid id { get; set; }
}

    [MessagePackObject]
    public class test1 : SaveData
    {
        [Key(1)]
        public int pizza { get; set; }
    }

    [MessagePackObject]
    public class test2 : SaveData
    {
        [Key(1)]
        public int pasta { get; set; }
    }

    [MessagePackObject]
    public class SaveInt : SaveData
    {
        [Key(1)]
        public int value { get; set; }
    }
}