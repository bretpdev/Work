#UTLWAGL.jcl  Daily aggregate loan limit reports.
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWAGL.LWAGLR1
   then
        rm ${reportdir}/ULWAGL.LWAGLR1
fi
if test -a ${reportdir}/ULWAGL.LWAGLR2
   then
        rm ${reportdir}/ULWAGL.LWAGLR2
fi
if test -a ${reportdir}/ULWAGL.LWAGLR3
   then
        rm ${reportdir}/ULWAGL.LWAGLR3
fi

# run the program

sas ${codedir}/UTLWAGL.sas -log ${reportdir}/ULWAGL.LWAGLR1  -mautosource
