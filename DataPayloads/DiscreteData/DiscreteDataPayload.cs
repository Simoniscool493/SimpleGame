using System;
using System.Text;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteDataPayload
    {
        public int[] Data;
        public Type UnderlyingType;

        public int SingleItem
        {
            get
            {
                if(Data.Length==1)
                {
                    return Data[0];
                }

                throw new Exception();
            }
        }

        public DiscreteDataPayload(Type underlyingType,int[] data)
        {
            UnderlyingType = underlyingType;
            Data = data;
        }

        public DiscreteDataPayload(Type underlyingType, int data)
        {
            UnderlyingType = underlyingType;
            Data = new int[] { data };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach(int d in Data)
            {
                sb.Append(d + " ");
            }

            return sb.ToString();
        }


        public override bool Equals(object obj)
        {
            var x  = (obj as DiscreteDataPayload).Data;
            var y = this.Data;

            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int result = 17;
            for (int i = 0; i < this.Data.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + this.Data[i];
                }
            }
            return result;
        }
    }
}
