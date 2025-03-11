using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;

public static class Extension
{
    private static CancellationTokenSource cancellationTokenSource;

    public static async void CountingTo(this TextMeshProUGUI targetText, int targetAmount, string format)
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new();
        var token = cancellationTokenSource.Token;

        var currentAmount = int.Parse(targetText.text.Replace(",", ""));
        if (currentAmount == targetAmount) return;

        var incrementing = currentAmount < targetAmount;

        try
        {
            while (incrementing ? currentAmount < targetAmount : currentAmount > targetAmount)
            {
                token.ThrowIfCancellationRequested();

                if (incrementing)
                {
                    if (currentAmount + 100 < targetAmount) currentAmount += 100;
                    else if (currentAmount + 10 < targetAmount) currentAmount += 10;
                    else currentAmount++;
                }
                else
                {
                    if (currentAmount - 100 > targetAmount) currentAmount -= 100;
                    else if (currentAmount - 10 > targetAmount) currentAmount -= 10;
                    else currentAmount--;
                }

                targetText.text = string.Format(format, currentAmount);
                await UniTask.Yield();
            }
        }
        catch (OperationCanceledException)
        {
            // 작업이 취소되었을 때의 처리 (로그 출력 등)
            UnityEngine.Debug.Log("Counting operation was canceled.");
            targetText.text = string.Format(format, targetAmount);
        }
    }
}
