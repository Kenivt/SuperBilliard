using Google.Protobuf;
using System.ComponentModel;
using System.IO;
using System;
using SuperBilliardServer.Tools;

namespace SuperBilliardServer.Tools
{
    public static class ProtobufHelper
    {
        public static byte[] ToBytes(object message)
        {
            return ((IMessage)message).ToByteArray();
        }

        public static void ToStream(object message, MemoryStream stream)
        {
            ((IMessage)message).WriteTo(stream);
        }

        public static object FromBytes(Type type, byte[] bytes, int index, int count)
        {
            object message = Activator.CreateInstance(type);
            ((IMessage)message).MergeFrom(bytes, index, count);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }

        public static object FromBytes(object instance, byte[] bytes, int index, int count)
        {
            object message = instance;
            ((IMessage)message).MergeFrom(bytes, index, count);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }

        public static object FromStream(Type type, MemoryStream stream)
        {
            object message = Activator.CreateInstance(type);
            ((IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)stream.Length);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }

        public static object FromStream(object message, MemoryStream stream)
        {
            // 这个message可以从池中获取，减少gc
            ((IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)stream.Length);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }
    }
}