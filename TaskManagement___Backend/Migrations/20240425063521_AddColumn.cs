using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement_April_.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Task_TaskId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Priority_priority",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Status_Status",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_SubTask_subTaskId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_User_AssignBy",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_User_AssignTo",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Task_subTaskId",
                table: "Tasks",
                newName: "IX_Tasks_subTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_Status",
                table: "Tasks",
                newName: "IX_Tasks_Status");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_priority",
                table: "Tasks",
                newName: "IX_Tasks_priority");

            migrationBuilder.RenameIndex(
                name: "IX_Task_AssignTo",
                table: "Tasks",
                newName: "IX_Tasks_AssignTo");

            migrationBuilder.RenameIndex(
                name: "IX_Task_AssignBy",
                table: "Tasks",
                newName: "IX_Tasks_AssignBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Project",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TaskCode",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Tasks_TaskId",
                table: "Comment",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Priority_priority",
                table: "Tasks",
                column: "priority",
                principalTable: "Priority",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Project_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Status_Status",
                table: "Tasks",
                column: "Status",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SubTask_subTaskId",
                table: "Tasks",
                column: "subTaskId",
                principalTable: "SubTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_User_AssignBy",
                table: "Tasks",
                column: "AssignBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_User_AssignTo",
                table: "Tasks",
                column: "AssignTo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Tasks_TaskId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Priority_priority",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Project_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Status_Status",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SubTask_subTaskId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_User_AssignBy",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_User_AssignTo",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TaskCode",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_subTaskId",
                table: "Task",
                newName: "IX_Task_subTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_Status",
                table: "Task",
                newName: "IX_Task_Status");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Task",
                newName: "IX_Task_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_priority",
                table: "Task",
                newName: "IX_Task_priority");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignTo",
                table: "Task",
                newName: "IX_Task_AssignTo");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignBy",
                table: "Task",
                newName: "IX_Task_AssignBy");

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "Task",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Task_TaskId",
                table: "Comment",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Priority_priority",
                table: "Task",
                column: "priority",
                principalTable: "Priority",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Status_Status",
                table: "Task",
                column: "Status",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_SubTask_subTaskId",
                table: "Task",
                column: "subTaskId",
                principalTable: "SubTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_User_AssignBy",
                table: "Task",
                column: "AssignBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_User_AssignTo",
                table: "Task",
                column: "AssignTo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
