using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class Version
    {
        [BinarySerializer.BinaryFormat]
        public int Major { get; set; }

        [BinarySerializer.BinaryFormat]
        public int Minor { get; set; }

        [BinarySerializer.BinaryFormat]
        public int Build { get; set; }

        public string Value
        {
            get { return $"{Major}.{Minor}.{Build}"; }
            set
            {
                string[] splitted = value.Split('.');

                if (splitted.Count() != 3)
                {
                    throw new InvalidOperationException($"Version {value} is invalid : must be at format Major.Minor.Build");
                }

                Major = int.Parse(splitted[0]);
                Minor = int.Parse(splitted[1]);
                Build = int.Parse(splitted[2]);
            }
        }

        public static bool operator<(Version left, Version right)
        {
            return (
                left.Major < right.Major
                || (
                    left.Major == right.Major
                    && left.Minor < right.Minor
                )
                || (
                    left.Major == right.Major
                    && left.Major == right.Minor
                    && left.Build < right.Build
                )
            );
        }

        public static bool operator>(Version left, Version right)
        {
            return (
                left.Major > right.Major
                || (
                    left.Major == right.Major
                    && left.Minor > right.Minor
                ) || (
                    left.Major == right.Major
                    && left.Minor == right.Minor
                    && left.Build > right.Build
                )
            );
        }

        public static bool operator==(Version left, Version right)
        {
            return left.Major == right.Major
                && left.Minor == right.Minor
                && left.Build == right.Build;
        }

        public static bool operator!=(Version left, Version right)
        {
            return !(left == right);
        }
    }
}
