using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [Header("Cursors")]
    [SerializeField] int one;
    [SerializeField] CursorData defaultCursor;
    [SerializeField] List<CursorData> _cursors;

    public void Set(string type = null){
        CursorData cursor = new CursorData();
        if(type == null){
            cursor = defaultCursor;
        }else{
            for (int i = 0; i < _cursors.Count; i++){
                if (_cursors[i].name == type){
                    cursor = _cursors[i];
                    i = _cursors.Count;
                }
            }
        }

        Cursor.SetCursor(cursor.cursorSprite, new Vector2(cursor.offsetX, cursor.offsetY), CursorMode.Auto);
    }

    [System.Serializable]
    public class CursorData {
        public string name;
        public Texture2D cursorSprite;
        public int offsetX;
        public int offsetY;
    }
}