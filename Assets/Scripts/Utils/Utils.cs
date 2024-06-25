public static class Utils {
    public static void Say(string text, float duration = 5f, AudioClipName voiceClipName = AudioClipName.None) {
        MessageToSay.Instance.SayLine(text, duration, voiceClipName);
    }
}