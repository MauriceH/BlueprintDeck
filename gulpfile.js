const {
  createBuildConfig,
  taskDotnetBuildCommon,
  taskDotnetSolutionTarget,
  taskDotnetPublishCommon,
  taskDotnetTestCommon,
  taskMakeRelease,
  taskPrepareCommon,
  taskVersion,
  taskDotnetNuGetPushCommon,
  taskDotnetPackCommon
} = require("@validdata.de/buildsystem");
const path = require("path");
const { series } = require("gulp");

const solutionDir = path.resolve("source", "dotnet");
const solution = path.join(solutionDir, "Blueprint.sln");

const meta = {
  productName: "BlueprintDock",
  companyName: "Maurice144"
};

const config = createBuildConfig();

const prepare = taskPrepareCommon(config);
const version = taskVersion(config);
const target = taskDotnetSolutionTarget(meta, config, solutionDir);
const compile = taskDotnetBuildCommon(solution, config);
const pack1 = taskDotnetPackCommon(path.join(solutionDir, 'BlueprintDeck.Core'),config)
const pack2 = taskDotnetPackCommon(path.join(solutionDir, 'BlueprintDeck.ASPNetCore'),config)
//const publish1 = taskDotnetPublishCommon(path.join(solutionDir, 'Schares.Onlineplaner.BackendService'), config);
//const publish2 = taskDotnetPublishCommon(path.join(solutionDir, 'Schares.Onlineplaner.SyncService'), config);
//const test = taskDotnetTestCommon(solution, config);

const init = series(prepare, version, target);
const build = series(compile, pack1, pack2);//, test, publish1, publish2);

exports.default = series(init, build);
exports.build = series(init, build);
exports.release = taskMakeRelease(config);
