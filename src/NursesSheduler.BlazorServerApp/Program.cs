﻿using Microsoft.Data.Sqlite;
using NursesScheduler.BlazorShared;
using NursesScheduler.BusinessLogic;
using NursesScheduler.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBusinessLogicLayer();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//add layers here
builder.Services.AddPresentationLayer();
builder.Services.AddBusinessLogicLayer();
builder.Services.AddInfrastructureLayer(new SqliteConnectionStringBuilder
                                                {
                                                    DataSource = "", //put db file location here
                                                    Mode = SqliteOpenMode.ReadWrite,
                                                    Password = "", //put db password here
}.ToString());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
