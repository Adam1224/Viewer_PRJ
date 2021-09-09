// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Media.Imaging;

namespace PhotoViewerDemo
{
    public class ExifMetadata
    {
        private readonly BitmapMetadata _metadata;

        public ExifMetadata(Uri imageUri)
        {
            var frame = BitmapFrame.Create(imageUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            _metadata = (BitmapMetadata)frame.Metadata;
        }


        public uint? Width
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40962}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof(uint))
                {
                    return (uint?)val;
                }
                return Convert.ToUInt32(val);
            }
        }

        public uint? Height
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40963}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof(uint))
                {
                    return (uint?)val;
                }
                return Convert.ToUInt32(val);
            }
        }

        public decimal? HorizontalResolution
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=282}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }
                return null;
            }
        }

        public decimal? ImageWidth
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=256}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }

                return null;
            }
        }

        public decimal? VerticalResolution
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=283}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }
                return null;
            }
        }

        public string EquipmentManufacturer
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=271}");
                return (val != null ? (string)val : string.Empty);
            }
        }

        public string CameraModel
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=272}");
                return (val != null ? (string)val : string.Empty);
            }
        }

        public string CreationSoftware
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=305}");
                return (val != null ? (string)val : string.Empty);
            }
        }



        public decimal? ExposureTime
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }
                return null;
            }
        }

        public decimal? ExposureCompensation
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=37380}");
                if (val != null)
                {
                    return ParseSignedRational((long)val);
                }
                return null;
            }
        }

        public decimal? LensAperture
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=33437}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }
                return null;
            }
        }

        public decimal? FocalLength
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=37386}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong)val);
                }
                return null;
            }
        }

        public ushort? IsoSpeed => (ushort?)QueryMetadata("/app1/ifd/exif/subifd:{uint=34855}");



        public DateTime? DateImageTaken
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=36867}");
                if (val == null)
                {
                    return null;
                }
                var date = (string)val;
                try
                {
                    return new DateTime(
                        int.Parse(date.Substring(0, 4)), // year
                        int.Parse(date.Substring(5, 2)), // month
                        int.Parse(date.Substring(8, 2)), // day
                        int.Parse(date.Substring(11, 2)), // hour
                        int.Parse(date.Substring(14, 2)), // minute
                        int.Parse(date.Substring(17, 2)) // second
                        );
                }
                catch (FormatException)
                {
                    return null;
                }
                catch (OverflowException)
                {
                    return null;
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
        }

        private decimal ParseUnsignedRational(ulong exifValue) => (exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0xFFFFFFFF00000000L) >> 32);

        private decimal ParseSignedRational(long exifValue) => (exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0x7FFFFFFF00000000L) >> 32);

        private object QueryMetadata(string query)
        {

            return null;
        }
    }
}