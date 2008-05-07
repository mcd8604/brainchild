using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
{
    class DoorModel : StaticModel
    {

        public DoorModel() { }

        public DoorModel(StaticModel p_Model) : base(p_Model) { }

        public DoorModel(String p_Name, String fileName, String audioName, TextureInfo p_TextureKey, List<short> rooms)
            :base(p_Name, fileName, audioName, p_TextureKey, rooms)
        {
            
        }

        public void DoorOpen()
        {
        }
    }
}
