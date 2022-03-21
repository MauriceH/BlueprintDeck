using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Properties;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node.Default;

[Node]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class FixedTimeTimerNode : INode
{
    private readonly ILogger<FixedTimeTimerNode> _logger;
    private readonly Design.Node _design;
    private System.Threading.Timer? _timer;
    private DateTime? _lastRun;

    public FixedTimeTimerNode(ILogger<FixedTimeTimerNode> logger, Design.Node design)
    {
        _logger = logger;
        _design = design ?? throw new ArgumentNullException(nameof(design));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public IInput<TimeSpan?>? TriggerTime { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public IOutput? Trigger { get; set; }
    
    [IncludeProperty]
    public TimeSpan? TriggerTimeMinimum { get; set; }
    
    [IncludeProperty]
    public TimeSpan? TriggerTimeMaximum { get; set; }
    

    public Task Activate()
    {
        _timer = new System.Threading.Timer(OnTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        if(TriggerTimeMinimum.HasValue)
        {
            _logger.LogDebug("Node {nodeTitle} triggers not before {minimumTriggerTime}", _design.Title, TriggerTimeMinimum);
        }

        if(TriggerTimeMaximum.HasValue)
        {
            _logger.LogDebug("Node {nodeTitle} triggers not after {maximumTriggerTime}", _design.Title, TriggerTimeMaximum);
        }
        return Task.CompletedTask;
    }

    public Task Deactivate()
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private void OnTimer(object? state)
    {
        var trigger = TriggerTime?.LastValue;
        if (!trigger.HasValue) return;
        if (TriggerTimeMinimum.HasValue && trigger.Value < TriggerTimeMinimum.Value)
        {
            trigger = TriggerTimeMinimum.Value;
        }
        if (TriggerTimeMaximum.HasValue && trigger.Value > TriggerTimeMaximum.Value)
        {
            trigger = TriggerTimeMaximum.Value;
        }
        _lastRun ??= trigger.Value < DateTime.Now.TimeOfDay ? DateTime.Now : DateTime.Now.AddDays(-1);

        if (trigger > DateTime.Now.TimeOfDay) return;
        if (_lastRun.Value.Date == DateTime.Now.Date) return;
        _lastRun = DateTime.Now;
        Task.Run(() => { Trigger?.Emit(); });
    }
}