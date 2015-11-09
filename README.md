# Seq.App.EventFieldDigest

Seq App for creating a digest containing all different values for a specific event property. The digest is generated based on a given interval, and the digest is emitted to the event log as a separate event. 

## Changes

### 1.1.0

- The emitted events are now tagged with a property "InstanceName" which contains the name of the instance creating the event. This may break previous signals in Seq.

## Building NuGet Package

From solution root, run:

- msbuild
- nuget pack ./Seq.App.EventFieldDigest/Seq.App.EventFieldDigest.csproj