using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtils
{
    public static bool StartsWithVowel( this string str )
    {
        bool startsWithAnyVowel =
            str.StartsWith( "A" )
            || str.StartsWith( "a" )
            || str.StartsWith( "E" )
            || str.StartsWith( "e" )
            || str.StartsWith( "I" )
            || str.StartsWith( "i" )
            || str.StartsWith( "O" )
            || str.StartsWith( "o" )
            || str.StartsWith( "U" )
            || str.StartsWith( "u" );

        return startsWithAnyVowel;
    }
}