using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksGame.Base
{
    internal class ShowTextState : BaseGameState
    {
        public string text { get; set; }

        private float _duration = 1f;
        private float _timeLeft = 0f;

        public ShowTextState(float duration) : this(string.Empty, duration) { }

        public ShowTextState(string text, float duration)
        {
            this.text = text;
            _duration = duration;

            Reset();
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            var textHalfLength = text.Length / 2;
            var textY = renderer.height / 2;
            var textX = renderer.width / 2 - textHalfLength;
            renderer.DrawString(text, textX, textY, ConsoleColor.White);
        }

        public override void Reset()
        {
            _timeLeft = _duration;
        }

        public override void Update(float deltaTime)
        {
            _timeLeft -= deltaTime;
        }

        public override bool IsDone()
        {
            return _timeLeft <= 0f;
        }
    }
}
