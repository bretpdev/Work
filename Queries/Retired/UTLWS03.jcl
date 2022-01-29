#UTLWS03.jcl   
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS03.LWS03R1
   then
        rm ${reportdir}/ULWS03.LWS03R1
fi
if test -a /sas/whse/progrevw/ULWS03.LWS03R2
   then
        rm /sas/whse/progrevw/ULWS03.LWS03R2
fi
if test -a /sas/whse/progrevw/ULWS03.LWS03RZ
   then
        rm /sas/whse/progrevw/ULWS03.LWS03RZ
fi


# run the program

sas ${codedir}/UTLWS03.sas -log ${reportdir}/ULWS03.LWS03R1  -mautosource
