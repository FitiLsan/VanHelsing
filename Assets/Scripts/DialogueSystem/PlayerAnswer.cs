using System;

namespace DialogueSystem

{
    [Serializable]
    public class PlayerAnswer
    {
        public bool _end;
        public bool _isStartQuest;
        public bool _isEndQuest;
        public int _questId;
        public int _answerId;

        public string _text;
        public int _toNode;

        public PlayerAnswer(int _answerId,string _text, int _toNode, int _end, int _isStartQuest, int _isEndQuest, int _questId)
        {
            this._answerId = _answerId;
            this._text = _text;
            this._toNode = _toNode;
            if (_end == 0) this._end = false;
            if (_end == 1) this._end = true;
            if (_isStartQuest == 0) this._isStartQuest = false;
            if (_isStartQuest == 1) this._isStartQuest = true;
            if (_isEndQuest == 0) this._isEndQuest = false;
            if (_isEndQuest == 1) this._isEndQuest = true;
            this._questId = _questId;
        }

        public PlayerAnswer()
        {
        }
    }
}