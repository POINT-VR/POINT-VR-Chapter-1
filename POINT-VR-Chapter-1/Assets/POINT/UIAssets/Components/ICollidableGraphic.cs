/// <summary>
/// Represents a single-click UI element other than a slider
/// </summary>
public interface ICollidableGraphic
{
    /// <summary>
    /// What should happen when this UI element is clicked via the raycaster?
    /// </summary>
    void OnCast();
}