using System;
using System.Collections.Generic;

namespace DeadDog.Audio
{
    internal static class BinarySearchExtension
    {
        #region ListSearch

        /// <summary>
        ///     Searches the entire sorted <see cref="IList{T}" /> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the list.</typeparam>
        /// <param name="source">
        ///     The <see cref="IList{T}" /> to search.
        /// </param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="compare">
        ///     A function that compares two objects of type <typeparamref name="TSource" />.
        ///     This function should return less than zero if the first argument is smaller than the second;
        ///     greater than zero if the first argument is larger that the second;
        ///     otherwise zero.
        /// </param>
        /// <returns>
        ///     The zero-based index of item in the sorted <see cref="IList{T}" />, if item is found;
        ///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///     if there is no larger element, the bitwise complement of <see cref="IList{T}" />.Count.
        /// </returns>
        public static int BinarySearch<TSource>(this IList<TSource> source, TSource item, Comparison<TSource> compare)
        {
            return BinarySearch(source, 0, source.Count, item, compare);
        }

        /// <summary>
        ///     Searches a range of elements in the sorted <see cref="IList{T}" /> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the list.</typeparam>
        /// <param name="source">
        ///     The <see cref="IList{T}" /> to search.
        /// </param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="compare">
        ///     A function that compares two objects of type <typeparamref name="TSource" />.
        ///     This function should return less than zero if the first argument is smaller than the second;
        ///     greater than zero if the first argument is larger that the second;
        ///     otherwise zero.
        /// </param>
        /// <returns>
        ///     The zero-based index of item in the sorted <see cref="IList{T}" />, if item is found;
        ///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///     if there is no larger element, the bitwise complement of <see cref="IList{T}" />.Count.
        /// </returns>
        public static int BinarySearch<TSource>(this IList<TSource> source, int index, int count, TSource item, Comparison<TSource> compare)
        {
            if (count == 0)
                return ~index;

            // Implemented according to "Discrete Mathematics and Its Applications" page 172
            int cmp;
            int i = index;
            int j = index + count - 1;
            while (i < j)
            {
                int m = (i + j)/2;
                cmp = compare(item, source[m]);
                if (cmp > 0)
                    i = m + 1;
                else
                    j = m;
            }
            cmp = compare(item, source[i]);
            if (cmp == 0)
                return i;
            else if (cmp > 0)
                return ~(i + 1);
            else
                return ~i;
        }

        public static int BinarySearch<TSource, TFind>(this IList<TSource> source, TFind item, Comparison<TFind> compare, Converter<TSource, TFind> convert)
        {
            return BinarySearch(source, 0, source.Count, item, compare, convert);
        }

        public static int BinarySearch<TSource, TFind>(this IList<TSource> source, int index, int count, TFind item, Comparison<TFind> compare, Converter<TSource, TFind> convert)
        {
            if (count == 0)
                return ~index;

            // Implemented according to "Discrete Mathematics and Its Applications" page 172
            int cmp;
            int i = index;
            int j = index + count - 1;
            while (i < j)
            {
                int m = (i + j)/2;
                cmp = compare(item, convert(source[m]));
                if (cmp > 0)
                    i = m + 1;
                else
                    j = m;
            }
            cmp = compare(item, convert(source[i]));
            if (cmp == 0)
                return i;
            else if (cmp > 0)
                return ~(i + 1);
            else
                return ~i;
        }

        #endregion

        #region ArraySearch

        /// <summary>
        ///     Searches the entire sorted array for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the array.</typeparam>
        /// <param name="source">The array to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="compare">
        ///     A function that compares two objects of type <typeparamref name="TSource" />.
        ///     This function should return less than zero if the first argument is smaller than the second;
        ///     greater than zero if the first argument is larger that the second;
        ///     otherwise zero.
        /// </param>
        /// <returns>
        ///     The zero-based index of item in the sorted array, if item is found;
        ///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///     if there is no larger element, the bitwise complement of the length of the array.
        /// </returns>
        public static int BinarySearch<TSource>(this TSource[] source, TSource item, Comparison<TSource> compare)
        {
            return BinarySearch(source, 0, source.Length, item, compare);
        }

        /// <summary>
        ///     Searches a range of elements in the sorted array for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the array.</typeparam>
        /// <param name="source">The array to search.</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="compare">
        ///     A function that compares two objects of type <typeparamref name="TSource" />.
        ///     This function should return less than zero if the first argument is smaller than the second;
        ///     greater than zero if the first argument is larger that the second;
        ///     otherwise zero.
        /// </param>
        /// <returns>
        ///     The zero-based index of item in the sorted array, if item is found;
        ///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///     if there is no larger element, the bitwise complement of the length of the array.
        /// </returns>
        public static int BinarySearch<TSource>(this TSource[] source, int index, int count, TSource item, Comparison<TSource> compare)
        {
            if (count == 0)
                return ~index;

            // Implemented according to "Discrete Mathematics and Its Applications" page 172
            int cmp;
            int i = index;
            int j = index + count - 1;
            while (i < j)
            {
                int m = (i + j)/2;
                cmp = compare(item, source[m]);
                if (cmp > 0)
                    i = m + 1;
                else
                    j = m;
            }
            cmp = compare(item, source[i]);
            if (cmp == 0)
                return i;
            else if (cmp > 0)
                return ~(i + 1);
            else
                return ~i;
        }

        public static int BinarySearch<TSource, TFind>(this TSource[] source, TFind item, Comparison<TFind> compare, Converter<TSource, TFind> convert)
        {
            return BinarySearch(source, 0, source.Length, item, compare, convert);
        }

        public static int BinarySearch<TSource, TFind>(this TSource[] source, int index, int count, TFind item, Comparison<TFind> compare, Converter<TSource, TFind> convert)
        {
            if (count == 0)
                return ~index;

            // Implemented according to "Discrete Mathematics and Its Applications" page 172
            int cmp;
            int i = index;
            int j = index + count - 1;
            while (i < j)
            {
                int m = (i + j)/2;
                cmp = compare(item, convert(source[m]));
                if (cmp > 0)
                    i = m + 1;
                else
                    j = m;
            }
            cmp = compare(item, convert(source[i]));
            if (cmp == 0)
                return i;
            else if (cmp > 0)
                return ~(i + 1);
            else
                return ~i;
        }

        #endregion
    }
}