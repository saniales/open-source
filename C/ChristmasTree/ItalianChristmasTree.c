/*
 * ItalianChristmasTree.c
 * This file is part of ChristmasTree
 *
 * Copyright (C) 2016 - Alessandro Sanino
 *
 * ChristmasTree is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * AlberoNatale is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ChristmasTree. If not, see <http://www.gnu.org/licenses/>.
 */
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define C3 130
#define Db3 138
#define D3 146
#define Eb3 155
#define E3 164
#define F3 174
#define Gb3 185
#define G3 196
#define Ab3 207
#define A3 220
#define Bb3 233
#define B3 246

#define C4 261
#define Db4 277
#define D4 293
#define Eb4 311
#define E4 329
#define F4 349
#define Gb4 369
#define G4 392
#define Ab4 415
#define A4 440
#define Bb4 466
#define B4 493

#define C5 523
#define Db5 544
#define D5 587
#define Eb5 622
#define E5 659
#define F5 698
#define Gb5 739
#define G5 783
#define Ab5 830
#define A5 880
#define Bb5 932
#define B5 987

#define C6 1046
#define Db6 1108
#define D6 1174
#define Eb6 1244
#define E6 1318
#define F6 1396
#define Gb6 1479
#define G6 1567
#define Ab6 1661
#define A6 1760
#define Bb6 1864
#define B6 1975

#define C7 2093
#define Db7 2217
#define D7 2349
#define Eb7 2489
#define E7 2637
#define F7 2794
#define Gb7 2960
#define G7 3136
#define Ab7 3322
#define A7 3520
#define Bb7 3729
#define B7 3951

#ifdef _WIN32

#define full 900
#define half 450
#include <windows.h>
#include <dos.h>

#define RESET 0x00

#define BLACK 0x00
#define RED 0x0C
#define GREEN 0x02
#define YELLOW 0x0E
#define WHITE 0x0F
#define BROWN 0x06
#define CYAN 0x0B
#define MAGENTA 0x05

//color the cell of the console screen using the specified color code.
void Color(int color)
{
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hConsole, color);
}

//creates the music
DWORD WINAPI musichetta(LPVOID lpParam)
{
    while (1)
    {
        Beep(E5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(E5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(E5, half);
        Beep(G5, half);
        Beep(C5, half);
        Beep(D5, half);
        Beep(E5, half);

        Beep(C4, half);
        Beep(D4, half);
        Beep(E4, half);

        Beep(F5, half);
        Beep(F5, half);
        Beep(F5, half);

        Sleep(half);

        Beep(F5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(E5, half);
        Beep(D5, half);
        Beep(D5, half);
        Beep(E5, half);
        Beep(D5, half);

        Sleep(half);

        Beep(G5, half);

        Sleep(half);

        // -----------

        Beep(E5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(E5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(E5, half);
        Beep(G5, half);
        Beep(C5, half);
        Beep(D5, half);
        Beep(E5, half);

        Beep(C4, half);
        Beep(D4, half);
        Beep(E4, half);

        Beep(F5, half);
        Beep(F5, half);
        Beep(F5, half);

        Sleep(half);

        Beep(F5, half);
        Beep(E5, half);
        Beep(E5, half);

        Sleep(half);

        Beep(G5, half);
        Beep(G5, half);
        Beep(F5, half);
        Beep(D5, half);
        Beep(C5, full);

        Sleep(full);
    }
    return 0;
}

//creates the thread which executes the music
void CreateSideThread()
{
    DWORD dwGenericThread;
    HANDLE hThread1 = CreateThread(NULL, 0, &musichetta, NULL, 0, &dwGenericThread);
}

//rename of CreateSideThread for italian people
void creaThreadMusichetta()
{
    CreateSideThread();
}

//rename of Sleep for italian people
void Dormi(int time)
{
    Sleep(time);
}

//Clears the console screen.
void Clear()
{
    system("cls");
}

void gotoxy(short x, short y)
{
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    COORD position = {x, y};
    SetConsoleCursorPosition(hConsole, position);
}

#else

#define full 900
#define half 450
#include <unistd.h>
#include <curses.h>
#include <pthread.h>
#include <signal.h>

#define RESET 0

#define BLACK 0
#define RED 1
#define GREEN 2
#define YELLOW 3
#define BLUE 4
#define MAGENTA 5
#define CYAN 6
#define WHITE 7
#define BROWN 3

//creates the music
void *musichetta(void *arg)
{
    char temp[50];
    while (1)
    {
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", G5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", C5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", C4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", G5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        // -----------
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", G5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", C5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", C4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E4, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", E5, half);
        system(temp);
        sprintf(temp, "sleep %.3f", half / 1000.0);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", G5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", G5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", F5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", D5, half);
        system(temp);
        sprintf(temp, "beep -f %d -l %d", C5, full);
        system(temp);
        sprintf(temp, "sleep %.3f", full / 1000.0);
        system(temp);
    }
    return NULL;
}

//creates the thread which executes the music
int creaThreadMusichetta(pthread_t *ret)
{
    return pthread_create(ret, NULL, musichetta, NULL);
}

//rename of Sleep for italian people.
void Dormi(int time)
{
    char temp[10];
    sprintf(temp, "sleep %.2f", time / 1000.0);
    system(temp);
}

//Clears the console screen.
void Clear()
{
    system("clear");
}

//colors text on the console screen using specified params.
void textcolor(int attr, int fg, int bg)
{
    char command[13];
    /* Command is the control command to the terminal */
    sprintf(command, "%c[%d;%d;%dm", 0x1B, attr, fg + 30, bg + 40);
    printf("%s", command);
}

//uses textcolor to color the character being printed
void Color(int color)
{
    if (color == BROWN)
        textcolor(1, color, BLACK);
    else
        textcolor(RESET, color, BLACK);
}

#endif // _WIN32 API

int main(void)
{
    srand(time(NULL));
#ifdef _WIN32
    creaThreadMusichetta();
#else
    pthread_t ret;
    creaThreadMusichetta(&ret);
#endif
    int n = 17;
    int i, j, k, modalita = 0;
    while (1)
    {
        Dormi(400);
        Clear();
        if (modalita % 2)
            Color(WHITE);
        else
            Color(BLACK);
        printf("\n\n");
        for (i = 0; i < n * 3 / 4; i++)
            putchar(' ');
        printf("BUON NATALE!!!\n\n");
        for (i = 0; i < n - 4; i++)
        {
            for (j = 0; j < n - i; j++)
                putchar(' ');
            for (j = 0; j < 2 * i + 1; j++)
            {
                if (i == 0)
                {
                    Color(YELLOW);
                    printf("*\n");
                    for (k = 0; k < n - i - 2; k++)
                        putchar(' ');
                    printf("*****");
                    if (modalita % 2)
                        printf(" * *");
                    else
                        printf("  * *");
                    putchar('\n');
                    for (k = 0; k < n - i - 1; k++)
                        putchar(' ');
                    printf("* *");
                    if (modalita % 2)
                        printf("  * *");
                    else
                        printf(" * *");
                    putchar('\n');
                    for (k = 0; k < n - i - 2; k++)
                        putchar(' ');
                    printf("*   *\n");
                    for (k = 0; k < n - i; k++)
                        putchar(' ');
                    Color(GREEN);
                }
                else if (j % 7 - i % 2 == 0)
                {
                    if (modalita % 2)
                        Color(WHITE);
                    else
                        Color(RED);
                }
                else if (rand() % 40 == 0)
                    if (modalita % 2)
                        Color(YELLOW);
                    else
                        Color(CYAN);
                else
                    Color(GREEN);
                putchar('*');
            }
            putchar('\n');
        }
        Color(BROWN);
        for (i = 0; i < n / 4; i++)
        {
            for (j = 0; j < (n - n / 8); j++)
                putchar(' ');
            for (j = 0; j < (n / 4 + 1); j++)
                putchar('*');
            putchar('\n');
        }
        modalita++;
    }
    return 0;
}
