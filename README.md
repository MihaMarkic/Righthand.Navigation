# Righthand.Navigation

[![NuGet](https://img.shields.io/nuget/v/Righthand.Navigation.svg)](https://www.nuget.org/packages/Righthand.Navigation)

An open source library that provides support for a navigation in a MVVM application.

Supports .NET Standard 2.0 target.

## History

* No releases yet

## Objectives

Goal of this project is to create a cross platform lightweight navigation library primarily, but not limited to, mobile devices.

## Samples

Sample is for Android and iOS and share common base (MVVM).

**Navigation**: Sample shows three differently colored fragments based on MVVM. First two fragments have button to navigate forward, backward navigation is achieved using Back button. An animation is used to show navigation direction. 

**Getting result from navigate page**: Third page contains a text field and when navigated back to second page, the content of that text field is retrieved and shown.

**Go back manually**: Click *Go Back* button on second page.

**Clear navigation stack**: Click *Clear navigation stack* button on second page.

### How to run

Android sample uses Debug/Any CPU context, while iOS sample uses Debug/iPhoneSimulator. Other contexts are not tested.