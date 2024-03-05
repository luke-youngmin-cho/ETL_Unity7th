using System;
using System.Collections.Generic;

namespace DiceGame.Data
{
    /// <summary>
    /// 저장소의 기본 상호작용
    /// </summary>
    /// <typeparam name="T"> 저장소에서 취급하는 데이터 형식 </typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// 데이터가 변경되었을때 알림 통지. int 번째 아이템이 T 로 변경되었음.
        /// </summary>
        event Action<int, T> onItemUpdated;

        /// <summary>
        /// 모든 데이터 순회용
        /// </summary>
        IEnumerable<T> GetAllItems();

        /// <summary>
        /// ID 로 데이터 검색
        /// </summary>
        /// <param name="id"> 검색할 데이터의 ID </param>
        /// <returns> 검색된 데이터 </returns>
        T GetItemByID(int id);

        /// <summary>
        /// 데이터 삽입
        /// </summary>
        /// <param name="item"> 삽입할 데이터 </param>
        void InsertItem(T item);

        /// <summary>
        /// 데이터 삭제
        /// </summary>
        /// <param name="item"> 삭제할 데이터 </param>
        void DeleteItem(T item);

        /// <summary>
        /// ID 에 해당하는 데이터 갱신
        /// </summary>
        /// <param name="id"> 검색할 ID </param>
        /// <param name="item"> 갱신할 데이터 </param>
        void UpdateItem(int id, T item);

        /// <summary>
        /// 데이터 변동내용 DB에 일괄 저장
        /// </summary>
        void Save();
    }
}