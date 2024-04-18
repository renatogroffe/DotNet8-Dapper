using BenchmarkDotNet.Running;
using BenchmarkingDapperEFCoreCRMPostgres.Tests;

new BenchmarkSwitcher(new [] { typeof(CRMTests) }).Run(args);