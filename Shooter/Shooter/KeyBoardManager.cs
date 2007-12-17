using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class KeyBoardManager
    {
        private static Hashtable mKeyMap;

        public static void iniKeyboard(){
            if (mKeyMap == null)
            {
                mKeyMap = new Hashtable();
            }
        }

        public static void addMapping(Keys inputKey ,KeyBoardAction keyAction)
        {
            if (mKeyMap.ContainsKey(inputKey))
            {
                mKeyMap[inputKey] = keyAction;
            }
            else
            {
                mKeyMap.Add(inputKey, keyAction);
            }
        }

        public static KeyBoardAction getMapping(Keys inputKey){

            return ((KeyBoardAction)(mKeyMap[inputKey]));
        }

        public static void checkKeyBoard(float elapsed){

            KeyboardState aKeyboard = Keyboard.GetState();
            
            //Get the current keys being pressed
            Keys[] aCurrentKeys = aKeyboard.GetPressedKeys();
            
            //Cycle through all of the keys being pressed and move the sprite accordingly
            for (int aCurrentKey = 0; aCurrentKey < aCurrentKeys.Length; aCurrentKey++)
            {
                if (aCurrentKeys[aCurrentKey] == Keys.Enter)
                {

                }
                if (mKeyMap.ContainsKey(aCurrentKeys[aCurrentKey]))
                {
                    ((KeyBoardAction)(mKeyMap[aCurrentKeys[aCurrentKey]])).invoke(elapsed);
                }
            }

        }

    }
}
