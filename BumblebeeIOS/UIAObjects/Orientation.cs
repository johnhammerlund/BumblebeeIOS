namespace BumblebeeIOS.UIAObjects
{
    public class Orientation
    {
        private readonly string _orientation;
        private Orientation(string orientation)
        {
            _orientation = orientation;
        }

        public static Orientation Landscape_Left { get { return new Orientation("UIA_DEVICE_ORIENTATION_LANDSCAPELEFT"); } }
        public static Orientation Landscape_Right { get { return new Orientation("UIA_DEVICE_ORIENTATION_LANDSCAPERIGHT"); } }
        public static Orientation Portrait { get { return new Orientation("UIA_DEVICE_ORIENTATION_PORTRAIT"); } }
        public static Orientation Portrait_Upsidedown { get { return new Orientation("UIA_DEVICE_ORIENTATION_PORTRAIT_UPSIDEDOWN"); } }
        public static Orientation Faceup { get { return new Orientation("UIA_DEVICE_ORIENTATION_FACEUP"); } }
        public static Orientation Facedown { get { return new Orientation("UIA_DEVICE_ORIENTATION_FACEDOWN"); } }
        public static Orientation Unknown { get { return new Orientation("UIA_DEVICE_ORIENTATION_UNKNOWN"); } }

        public override string ToString()
        {
            return _orientation;
        }
    }
}
