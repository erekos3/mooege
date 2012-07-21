/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(4580)]
    class Leah : InteractiveNPC
    {
        public Leah(World world, int snoID, TagMap tags)
            : base(world, snoID, tags)
        { }

        // One of the Leah (the one supposed to follw the player in the rescue cain quest 
        // have a funny conversationID 138264 which is not found I'll replace it with the 198541 conv id
        protected override void ReadTags()
        {
        //    var snoConversationList = Tags[MarkerKeys.ConversationList].Id; // this is the wrong conversation list 

        //    if (snoConversationList == 138264)
        //    {
        //        Logger.Debug(" (Leah ReadTags) name and ID of conv with wrong stuff {0} - {1} ", Tags[MarkerKeys.ConversationList].Name, Tags[MarkerKeys.ConversationList].Id);

        //        Tags.Replace(new TagKeySNO(MarkerKeys.ConversationList.ID), new TagMapEntry(MarkerKeys.ConversationList.ID, 198541, 2)); // brute force thingy isn't it ?      

        //        Logger.Debug(" (Leah ReadTags) name and ID of conv with corrected stuff {0} - {1} ", Tags[MarkerKeys.ConversationList].Name, Tags[MarkerKeys.ConversationList].Id);

                

        //    }

            //if (snoConversationList == 138264)
            //{
            //    Logger.Debug(" (Leah ReadTags) name of conv with wrong stuff {0}", Tags[MarkerKeys.ConversationList].Name);
            //    Logger.Debug(" (Leah ReadTags) modifying the tag map DIRECTLY ");
            //    Tags.Add(MarkerKeys.ConversationList, new TagMapEntry( MarkerKeys.ConversationList.ID, 198541, 2));
                
                
            //    //Tags.Add(new TagKeySNO(198541), new TagMapEntry ( , , 2)
            //    //[MarkerKeys.ConversationList].Id = 198541;
            //    //ConversationList = Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.ConversationList][198541].Data as Mooege.Common.MPQ.FileFormats.ConversationList;
            //}

            // now just fill the actor's ConversationList
            base.ReadTags();
        }
    }
}
