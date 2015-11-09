# Seq.App.EventFieldDigest

Creates a digest event of a event types unique field value for a defined time intervall.

## Changes

### 1.1.0

- The emitted events are now tagged with a property "InstanceName" which contains the name of the instance creating the event. This may break previous signals in Seq.

## Building NuGet Package

From solution root, run:

- msbuild
- nuget pack ./Seq.App.EventFieldDigest/Seq.App.EventFieldDigest.csproj