# How to contribute

One of the easiest ways to contribute is to participate in discussions on GitHub issues. You can also contribute by submitting pull requests with code changes.

## General feedback and discussions?
Start a discussion on the [SqliteWasmHelper issue tracker](https://github.com/JeremyLikness/SqliteWasmHelper/issues).

## Bugs and feature requests?
To request a feature or report a new bug, [file a new issue](https://github.com/JeremyLikness/SqliteWasmHelper/issues/new/choose) and follow the template suggestions.

## Contributing code and content

We accept fixes and features! Here are some resources to help you get started on how to contribute code or new content.

* ["Help wanted" issues](https://github.com/JeremyLikness/SqliteWasmHelper/labels/help%20wanted) - these issues are up for grabs. Comment on an issue if you want to create a fix.
* ["Good first issue" issues](https://github.com/JeremyLikness/SqliteWasmHelper/labels/good%20first%20issue) - we think these are a good for newcomers.

### Identifying the scale

If you would like to contribute to one of our repositories, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix) feel 
free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with the team and ensure it follows the product roadmap. 
You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza 
and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. All code submissions will be rigorously reviewed 
and tested by the Expression Power Tools team before being merged into the source.

### Submitting a pull request

If you don't know what a pull request is read this article: https://help.github.com/articles/using-pull-requests. Make sure the repository can build and all tests pass. 
Familiarize yourself with the project workflow and our coding conventions. The project contains an [`.editorConfig`](https://github.com/JeremyLikness/SqliteWasmHelper/blob/main/SqliteWasmHelper/.editorconfig)
that contains rules for conventions and is recognized automatically by Visual Studio. 

In general, the checklist for pull requests is:

- Check with the maintainers for larger requests
- Ensure the project compiles in _both_ `Debug` and `Release` modes
- Write tests to adequately cover your functionality
- All files added should have complete XML documentation with examples where appropriate and the required heading

Project maintainers will review PRs and merge/update versions as appropriate.

### Tests

-  Tests need to be provided for every bug/feature that is completed.
-  Tests only need to be present for issues that need to be verified by QA (for example, not tasks)
-  If there is a scenario that is far too hard to test there does not need to be a test for it.
  - "Too hard" is determined by the team as a whole.

## Code of conduct

See [Code of Conduct](./CODE_OF_CONDUCT.md)
