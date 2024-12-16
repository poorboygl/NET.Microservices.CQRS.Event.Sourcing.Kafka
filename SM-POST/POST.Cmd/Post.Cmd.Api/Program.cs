using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.infrastructure;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository,EventStoreRepository>();
builder.Services.AddScoped<IEventStore,EventStores>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>,EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//register command handler methods
var CommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandle<NewPostCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<EditMessageCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<LikePostCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<AddCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<EditCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<RemoveCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandle<DeletePostCommand>(CommandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
