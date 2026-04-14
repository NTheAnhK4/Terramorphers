using System;

using UnityEngine;

namespace GameCore.Utility.Bit
{
    public class BigBitSet 
    {
        private ulong[] _data;

        public BigBitSet(int initialBits = 64)
        {
            int size = (initialBits + 63) / 64;
            _data = new ulong[size];
        }

        private void EnsureCapacity(int bitIndex)
        {
            int requiredIndex = bitIndex / 64;
            if (requiredIndex < _data.Length)
                return;

            int newSize = Math.Max(_data.Length * 2, requiredIndex + 1);
            Array.Resize(ref _data, newSize);
        }

        public void SetBit(int index)
        {
            EnsureCapacity(index);
            _data[index / 64] |= (1UL << (index % 64));
        }

        public bool IsSet(int index)
        {
            if (index / 64 >= _data.Length)
                return false;

            return (_data[index / 64] & (1UL << (index % 64))) != 0;
        }

        // =========================
        // 🔥 SAVE / LOAD
        // =========================

        public void Save(string key)
        {
            byte[] bytes = new byte[_data.Length * sizeof(ulong)];
            Buffer.BlockCopy(_data, 0, bytes, 0, bytes.Length);

            string base64 = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, base64);
            PlayerPrefs.Save();
        }

        public void Load(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return;

            string base64 = PlayerPrefs.GetString(key);
            byte[] bytes = Convert.FromBase64String(base64);

            int length = bytes.Length / sizeof(ulong);
            _data = new ulong[length];

            Buffer.BlockCopy(bytes, 0, _data, 0, bytes.Length);
        }
    }

}
